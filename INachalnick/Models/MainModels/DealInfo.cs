using ProgramUtilities.Mongo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INachalnick.Models
{
    public class DealInfo : MongoBaseDocument
    {
        [Required(ErrorMessage = "Price is required.")] public float Price { get; set; }
        public bool TaxIncluded { get; set; }
        public string? Discount { get; set; }
    }
}