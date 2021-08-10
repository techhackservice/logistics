using System;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class InvoiceRequest : IUseCaseRequest<AcknowledgementResponse>
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
        public string ErrorMsg { get; set; }
    }
    public class InvoiceDetails : IUseCaseRequest<AcknowledgementResponse>
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
        /*public string OrderName { get; set; }//order_name
        public DateTime OrderDate { get; set; }//order_date
        public DateTime ExpectedDate { get; set; }//expected_date
        public DateTime DeliveredDate { get; set; }//delivery_date
        public string OrderStatus { get; set; }//order_status - enum('conform','cancel','hold') public string CustomerId { get; set; }
        public string CustomerId { get; set; }//customer_id
        public string CustomerFirstName { get; set; }//customer_first_name
        public string CustomerMiddleName { get; set; }//customer_middle_name
        public string CustomerLastName { get; set; }//customer_last_name
        public string CustomerAddressId { get; set; }//shipping_address_id
        public string STDCodeLandlineNumber { get; set; }//std_code_landline_number
        public string CustomerLandlineNumber { get; set; }//landline_number
        public string STDCodeMobileNumber { get; set; }//std_code_mobile_number
        public string CustomerMobileNumber { get; set; }//mobile_number
        public string CustomerEmail { get; set; }//email
        public List<ProductDetails> ProductDetailsList { get; set; }*/
        public string TrackingId { get; set; }//tracking_id
        public string TrackingStatus { get; set; }//tracking_status - enum('shipping','pickup','shipped','intransit','outofdelivery','delivered','hold','return')
        public string CreatedBy { get; set; }//created_by
        public string ModifiedBy { get; set; }//modified_by

    }
}