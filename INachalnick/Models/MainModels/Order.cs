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
        public Order(DateTime createdAt, string orderType, string customerId, List<string> dealsInfo, List<string> invoices, string notes, StatusEnum status, List<string> receipts, string jobId, List<string> progressIds)
        {
            CreatedAt = createdAt;
            OrderType = orderType;
            CustomerId = customerId;
            DealsInfo = dealsInfo;
            Invoices = invoices;
            Notes = notes;
            Status = status;
            Receipts = receipts;
            JobId = jobId;
            ProgressIds = progressIds;
        }
    }
    public enum StatusEnum
    {
        Paid,
        Cancelled
    }
}