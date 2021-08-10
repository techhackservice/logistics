using System;
using System.Collections.Generic;

namespace Web.Api.Models.Request
{
    public class ProductDetails
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Discount { get; set; }
        public decimal SingleProductAmount { get; set; }
        public int ItemCount { get; set; }
        public string Size { get; set; }
        public decimal Weight { get; set; }
        public decimal TotalAmount { get; set; }
        public int GSTPercentage { get; set; }//gst_percentage
        public GSTDetails GSTDetails { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class GSTDetails
    {
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
    }
}