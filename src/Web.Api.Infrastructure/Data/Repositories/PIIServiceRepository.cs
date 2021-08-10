using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;
using Dapper;
using Web.Api.Core.Enums;
using Newtonsoft.Json;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Infrastructure.Data.Repositories
{
    internal sealed class PIIServiceRepository : IPIIServiceRepository
    {
        private readonly IHttpClientService _httpClientService;
        public PIIServiceRepository(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }
        public async Task<bool> GetContactAddressId(AddressContactDetails request)
        {
            var piiList = new PiiListAddressContactData();
            piiList.AddressId = new List<string>();
            piiList.ContactId = new List<string>();
            piiList.AddressId.Add(request.addressDetails.AddressId);
            piiList.ContactId.Add(request.contactDetails.ContactId);
            var piiRequest = JsonConvert.SerializeObject(piiList);
            //var response = await _httpClientService.Post("http://localhost:5004/v1/list-personal-information", piiRequest);
            var response = await _httpClientService.Post("http://pii.appplaza.io/v1/list-personal-information", piiRequest);

            var responseData = JsonConvert.DeserializeObject<PiiOverallDataResponse>(response);
            if (responseData.PiiDataResponses.Count() > 0)
            {
                request.addressDetails = responseData.PiiDataResponses.First().addressDetails[0];
                request.contactDetails = responseData.PiiDataResponses.First().contactDetails[0];
            }
            return responseData.Success;
        }
        public async Task<bool> CreateContactAddressId(AddressContactDetails request)
        {
            var addressContactDetails = new AddressContactDetails();
            addressContactDetails.addressDetails = request.addressDetails;
            addressContactDetails.contactDetails = request.contactDetails;
            var piiRequest = JsonConvert.SerializeObject(addressContactDetails);
            //var response = await _httpClientService.Post("http://localhost:5004/v1/personal-information", piiRequest);
            var response = await _httpClientService.Post("http://pii.appplaza.io/v1/personal-information", piiRequest);

            var responseData = JsonConvert.DeserializeObject<PiiResponse>(response);

            request.addressDetails.AddressId = responseData.piiResponseModel.AddressId;
            request.contactDetails.ContactId = responseData.piiResponseModel.ContactId;

            return responseData.Success;
        }
        public async Task<bool> UpdateContactAddressId(AddressContactDetails request)
        {
            var addressContactDetails = new AddressContactDetails();
            addressContactDetails.addressDetails = request.addressDetails;
            addressContactDetails.contactDetails = request.contactDetails;
            var piiRequest = JsonConvert.SerializeObject(addressContactDetails);
            //var response = await _httpClientService.Put("http://localhost:5004/v1/personal-information", piiRequest);
            var response = await _httpClientService.Put("http://pii.appplaza.io/v1/personal-information", piiRequest);

            var responseData = JsonConvert.DeserializeObject<PiiResponse>(response);

            return responseData.Success;
        }

    }
}