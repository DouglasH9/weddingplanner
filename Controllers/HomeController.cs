using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using weddingplanner.Models;

namespace weddingplanner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        public HomeController(MyContext context)
        {
            _context = context;
        }
        [HttpGet ("Index")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost ("Register")]
        public  IActionResult Register(User newUser)
        {
            if(_context.Users.Any(user => user.Email == newUser.Email))
            {
                ModelState.AddModelError("Email", "Email already in use!");
                return View("Index");
            }

            if(ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                _context.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View("Index");
        }

        [HttpGet ("Home")]
        public IActionResult Home()
        {
            int? loggedUserId = HttpContext.Session.GetInt32("LoggedUserId");
            if (loggedUserId == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.User = _context.Users.FirstOrDefault(user => user.UserId == loggedUserId);

            ViewBag.AllWeddings = _context.Weddings
            .Include(wedWithUser => wedWithUser.Creator)
            .Include(wedWithRsvps => wedWithRsvps.WeddingRsvps)
            .ToList();
            
            return View("Home");
        }

        [HttpPost ("LoginUser")]
        public IActionResult LoginUser(LoginUser checkUser)
        {
            if(ModelState.IsValid)
            {
                // find user with email
                User userInDb = _context.Users.FirstOrDefault(use => use.Email == checkUser.LoginEmail);
                // if user doesn't exist
                    // send validation error
                if(userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Login");
                    return View("Index");
                }
                // verify password
                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(checkUser, userInDb.Password, checkUser.LoginPassword);
                    // send validation error
                if(result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Invalid password");
                    return View("Index");
                }

                // put userId in session
                HttpContext.Session.SetInt32("LoggedUserId", userInDb.UserId);
                
                return RedirectToAction("Home");
            }
            return View("Index");
        }

        [HttpGet ("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet ("Plan")]
        public IActionResult Plan()
        {
            int? loggedUserId = HttpContext.Session.GetInt32("LoggedUserId");
            if (loggedUserId == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost ("PushPlan")]
        public IActionResult PushPlan(Wedding newWedding)
        {
            int? loggedUserId = HttpContext.Session.GetInt32("LoggedUserId");
            if (loggedUserId == null)
            {
                return RedirectToAction("Index");
            }
            if(ModelState.IsValid)
            {
                newWedding.UserId = (int)loggedUserId;
                _context.Add(newWedding);
                _context.SaveChanges();
                return RedirectToAction("Home");
            }
            return View("Plan");
        }

        [HttpGet ("/viewWed/{wedId}")]
        public  IActionResult ViewOneWed(int wedId)
        {
            int? loggedUserId = HttpContext.Session.GetInt32("LoggedUserId");
            if (loggedUserId == null)
                {
                    return RedirectToAction("Home");
                }
            ViewBag.singleWedding = _context.Weddings
                .Include(wed => wed.WeddingRsvps)
                    .ThenInclude(rsvp => rsvp.Guest)
                .FirstOrDefault(singleWed => singleWed.WeddingId == wedId);

            //loop through singleWedding.WeddingRsvps
                //each WeddingRsvp has a Guest, which is a user with name, whatever

            ViewBag.WedWithRsvpsAndUsers = _context.Users
                .Include(user => user.UserRsvps)
                    .ThenInclude(rsvp => rsvp.Wedding)
                .Where(user => user.UserRsvps.Any(userrsvps => userrsvps.WeddingId == wedId));
            

            return View("ViewWed");
        }

        [HttpGet ("rsvp/{wedId}")]
        public IActionResult Rsvp(int wedId)
        {
            int? loggedUserId = HttpContext.Session.GetInt32("LoggedUserId");
            if (loggedUserId == null)
                {
                    return RedirectToAction("Home");
                }

            Rsvp newRsvp = new Rsvp();
            newRsvp.UserId = (int)loggedUserId;
            newRsvp.WeddingId = wedId;
            _context.Add(newRsvp);
            _context.SaveChanges();

            return RedirectToAction("Home");
        }

        [HttpGet ("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

// ====================================================================
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
