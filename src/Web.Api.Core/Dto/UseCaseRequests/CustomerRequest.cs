using System;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class CustomerDetails
    {
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ShippingAddressId { get; set; }
        public AddressDetails ShippingAddressDetails { get; set; }
        public string DeliveryAddressId { get; set; }
        public AddressDetails DeliveryAddressDetails { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class AddressDetails
    {
        public string AddressId { get; set; }
        public string FirstLine { get; set; }
        public string SecondLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string Landmark { get; set; }
        public string GoogleMapLink { get; set; }
        public string STDCodeLandlineNumber { get; set; }
        public string LandlineNumber { get; set; }
        public string STDCodeMobileNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}