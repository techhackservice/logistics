using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IPIIServiceRepository 
    {
        Task<bool> GetContactAddressId(AddressContactDetails request);
        Task<bool> CreateContactAddressId(AddressContactDetails request);
        Task<bool> UpdateContactAddressId(AddressContactDetails request);
    }
}