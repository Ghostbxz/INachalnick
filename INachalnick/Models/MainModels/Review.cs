using ProgramUtilities.Mongo;
using System;
using System.ComponentModel.DataAnnotations;

namespace INachalnick.Models
{
    public class Review : MongoBaseDocument
    {

        public string ReviewName { get; set; }
        public DateTime ReviewDate { get; set; }
        public Review(string reviewName, DateTime reviewDate)
        {
            ReviewName = reviewName;
            ReviewDate = reviewDate;
        }
    }
}