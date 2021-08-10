using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IProductRepository 
    {
        Task<List<ProductDetails>> GetOrderProductDetails(string orderId);
        //Task<List<ProductDetails>> GetProductDetails(string productId);
    }
}