using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartGrid.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public string Consumer { get; set; }
        public string Producer { get; set; }
        public float KwhAmount { get; set; }
        public float PricePerKwh { get; set; }
        public float TotalPrice { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}