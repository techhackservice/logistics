using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface ICustomerRepository 
    {
        Task<List<CustomerDetails>> GetCustomerDetails(string customerId);
        Task<AddressDetails> GetAddressDetails(string addressId);
    }
}