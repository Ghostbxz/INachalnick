using ProgramUtilities.Mongo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace INachalnick.Models
{
    public class Customer : MongoBaseDocument
    {

        [Required(ErrorMessage = "FirstName is required.")] public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required.")] public string LastName { get; set; }
        public List<string> Contacts { get; set; } = new();
        public string? Avatar { get; set; }
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
        public Customer(string firstName, string lastName, List<string> contacts, string? avatar, DateTime firstSeen, DateTime lastSeen)
        {
            FirstName = firstName;
            LastName = lastName;
            Contacts = contacts;
            Avatar = avatar;
            FirstSeen = firstSeen;
            LastSeen = lastSeen;
        }
    }
}