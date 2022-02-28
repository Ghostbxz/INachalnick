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
        public Job(DateTime createdAt, string city, string address, Location location, string serialNumber)
        {
            CreatedAt = createdAt;
            City = city;
            Address = address;
            Location = location;
            SerialNumber = serialNumber;
        }

    }
}