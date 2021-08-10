namespace Web.Api.Models.Request
{
    public class InvoiceRequest
    {
        public string InvoiceId { get; set; }//invoice_id
        public string InvoiceType { get; set; }//enum('payable','receivable')
        public string InvoiceStatus { get; set; }//enum('open','close')
        public string ConvinienceFeesType { get; set; }//convinience_fees_type - enum('fixed','variant')
        public decimal ConvinienceFeesAmount { get; set; }//convinience_fees_amt - decimal(10,2)
        public decimal InvoiceAmount { get; set; }//invoice_amt
        public string CompanyId { get; set; }//company_id
        public string OrderId { get; set; }//order_id
        public OrderDetails OrderDetails { get; set; }
        public string TrackingId { get; set; }//tracking_id
        public bool IsUpdate { get; set; }
        public string CreatedBy { get; set; }//created_by
        public string ModifiedBy { get; set; }//modified_by
    }
}