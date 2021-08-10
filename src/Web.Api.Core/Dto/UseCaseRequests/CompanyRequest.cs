using System;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class CompanyRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyUrl { get; set; }
        //public string PartnerId { get; set; }
        public string TenantId { get; set; }
        public string SourceName { get; set; }
        public string GSTNumber { get; set; }
        public string TinNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string AddressId { get; set; }
        public AddressDetailsRequest AddressDetails { get; set; }
        public string ContactId { get; set; }
        public ContactDetailsRequest ContactDetails { get; set; }
        public bool IsUpdate {get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ErrorMsg { get; set; }
    }
    public class CompanyDetails
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyUrl { get; set; }
        //public string PartnerId { get; set; }
        public string TenantId { get; set; }
        public string SourceName { get; set; }
        public string GSTNumber { get; set; }
        public string TinNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string AddressId { get; set; }
        public AddressDetailsRequest AddressDetails { get; set; }
        public string ContactId { get; set; }
        public ContactDetailsRequest ContactDetails { get; set; }
        public ConvinienceFeesDetails ConvinienceFeesDetails { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ErrorMsg { get; set; }
    }
    public class AddressDetailsRequest
    {
        public string AddressTypeId { get; set; }
        public string AddressId { get; set;}
        public string FirstLine {  get; set; }
        public string SecondLine { get; set; }
        public string City {get; set;}
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
    }
    public class ContactDetailsRequest
    {
        public string ContactId { get; set; }
        public string WiredPhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string Email { get; set; }
        public string PreferredContactMode { get; set; }
        public string PreferredContactTime { get; set; }
    }
    public class AddressContactDetails : IUseCaseRequest<AcknowledgementResponse>
    {
        public AddressDetailsRequest addressDetails { get; set; }
        public ContactDetailsRequest contactDetails { get; set; }
    }    
    public class PiiListAddressContactData
    {
        public List<string>  AddressId { get; set; }
        public List<string> ContactId { get; set; }
    }

    public class ConvinienceFeesDetails : IUseCaseRequest<AcknowledgementResponse>
    {
        public string CompanyId { get; set; }//company_id
        public string CompanyName { get; set; }//company_name
        public string FeesType { get; set; }//convinience_fees_type - enum('fixed','variant')
        public decimal FeesAmount { get; set; }//convinience_fees_amt - decimal(10,2)
        public string ErrorMsg {get; set; }
        public string ModifiedBy { get; set; }//
    }
}