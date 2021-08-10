using System;
using System.Collections.Generic;

namespace Web.Api.Models.Request
{
    public class OrderDetails
    {
        public string OrderId { get; set; }
        public string OrderName { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CustomerId { get; set; }
        public CustomerDetails CustomerDetails { get; set; }
        public string UserId { get; set; }
        public List<ProductDetails> ProductDetailsList { get; set; }
        public string InvoiceId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime DeliveredDate { get; set; }
        public string ReciverName { get; set; }
        public string OrderStatus { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}