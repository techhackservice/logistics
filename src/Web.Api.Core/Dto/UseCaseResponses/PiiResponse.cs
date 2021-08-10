using System;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class PiiResponse
    {
        public PiiResponseModel piiResponseModel { get; set; }
        public bool Success { get; set; }
    }
    public class PiiResponseModel
    {
        public string AddressId { get; set; }
        public string ContactId { get; set; }
    }
    public class PiiOverallDataResponse 
    {
        public List<PiiDataResponses> PiiDataResponses { get; set; }
        public bool Success { get; set; }
    }
    public class PiiDataResponses
    {
        public List<AddressDetailsRequest> addressDetails { get; set; }
        public List<ContactDetailsRequest> contactDetails { get; set; }
    }
}