using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace weddingplanner.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}
        [Required]
        [EmailAddress]
        public string Email {get; set;}
        [Required]
        public string FName {get; set;}
        [Required]
        public string LName {get; set;}
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password should be at least 8 characters long")]
        public string Password {get; set;}
        [Required]
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string PassCon {get; set;}
        public List<Wedding> CreatedWeddings {get;set;}
        public List<Rsvp> UserRsvps {get;set;}
        // public List<Rsvp> UserRsvps {get;set;}
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
        public DateTime CreatedAt {get; set;} = DateTime.Now;
    }
}