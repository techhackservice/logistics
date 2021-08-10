using System;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class CompanyBranchRequest: IUseCaseRequest<AcknowledgementResponse>
    {
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string CompanyId { get; set; }
        public string DefaultBranch { get; set; }
        public string BranchUrl { get; set; }
        public string LicenseNumber { get; set; }
        public string AddressId { get; set; }
        public AddressDetailsRequest AddressDetails { get; set; }
        public string ContactId { get; set; }
        public ContactDetailsRequest ContactDetails { get; set; }
        public bool IsUpdate {get; set; }
        public string ErrorMsg { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class CompanyBranchDetails
    {
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string DefaultBranch { get; set; }
        public string BranchUrl { get; set; }
        public string LicenseNumber { get; set; }
        public string AddressId { get; set; }
        public AddressDetailsRequest AddressDetails { get; set; }
        public string ContactId { get; set; }
        public ContactDetailsRequest ContactDetails { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}