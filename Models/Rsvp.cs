using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace weddingplanner.Models
{
    public class Rsvp
    {
        [Key]
        public int RsvpId {get;set;}
        public int UserId {get;set;}
        public int WeddingId {get; set;}
        public List<User> Rsvpee {get;set;}
        public Wedding RsvpWedding {get; set;}
    }
}