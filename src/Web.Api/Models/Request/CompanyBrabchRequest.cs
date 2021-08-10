namespace Web.Api.Models.Request
{
    public class CompanyBranchRequest
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
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}