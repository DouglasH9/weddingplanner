using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace weddingplanner.Models
{

    public class Wedding
    {
        [Key]
        public int WeddingId {get;set;}
        [Required]
        [MinLength(2)]
        public string WedderOne {get;set;}
        [Required]
        [MinLength(2)]
        public string WedderTwo {get;set;}
        [Required]
        public DateTime WeddingDate {get;set;}
        [Required]
        [MinLength(7)]
        public string Address {get;set;}
        public int UserId {get;set;}
        public User Creator {get;set;}
        public List<Rsvp> WeddingRsvps {get; set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        internal object Include(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}