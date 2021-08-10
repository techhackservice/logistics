using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IOrderRepository 
    {
        Task<List<OrderDetails>> GetOrderDetails(string companyId, string orderId);//, string invoiceId);
    }
}