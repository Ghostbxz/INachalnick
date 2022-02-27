using ProgramUtilities.Mongo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INachalnick.Models
{
    public class Job : MongoBaseDocument
    {
        public DateTime CreatedAt { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public Location Location { get; set; }
        public string SerialNumber { get; set; }

    }
}