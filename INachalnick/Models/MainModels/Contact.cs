using ProgramUtilities.Mongo;
using System;
using System.ComponentModel.DataAnnotations;

namespace INachalnick.Models
{
    public class Contact : MongoBaseDocument
    {
        [Required(ErrorMessage = "Phone number is required.")] public string PhoneNumber { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
    }
}