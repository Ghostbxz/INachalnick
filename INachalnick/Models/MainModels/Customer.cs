using ProgramUtilities.Mongo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INachalnick.Models
{
    public class Customer : MongoBaseDocument
    {
        [Required(ErrorMessage = "FirstName is required.")] public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required.")] public string LastName { get; set; }
        public List<string> Contacts { get; set; }
        public string Avatar { get; set; }
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
    }
}