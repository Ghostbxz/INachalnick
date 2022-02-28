using ProgramUtilities.Mongo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INachalnick.Models
{
    public class Order : MongoBaseDocument
    {
        public DateTime CreatedAt { get; set; }
        [Required(ErrorMessage = "Name is required.")] public string OrderType { get; set; }
        public string CustomerId { get; set; }
        public List<string> DealsInfo { get; set; }
        public List<string> Invoices { get; set; }
        public string Notes { get; set; }
        public StatusEnum Status { get; set; }
        public List<string> Receipts { get; set; }
        public string JobId { get; set; }
        public List<string> ProgressIds { get; set; }
    }
    public enum StatusEnum
    {
        Paid,
        Cancelled
    }
}