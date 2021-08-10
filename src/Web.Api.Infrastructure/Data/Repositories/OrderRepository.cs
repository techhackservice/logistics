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
    internal sealed class OrderRepository : IOrderRepository
    {
        private ICustomerRepository _customerRepository;
        private IProductRepository _productRepository;
        private readonly AppDbContext _appDbContext;
        private readonly IHttpClientService _httpClientService;
        public OrderRepository(ICustomerRepository customerRepository, IProductRepository productRepository, AppDbContext appDbContext, IHttpClientService httpClientService)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _appDbContext = appDbContext;
            _httpClientService = httpClientService;
        }
        public async Task<List<OrderDetails>> GetOrderDetails(string companyId, string orderId)
        {
            List<OrderDetails> retOrderDetailsList = new List<OrderDetails>();

            try
            {
                var tableName = $"Logisticmate.order_obj oo, " +
                                $"Logisticmate.company_obj co ";

                var ColumAssign = $"oo.order_id as OrderId, oo.order_name as OrderName, " + 
                                  $"oo.user_id as Status, oo.customer_id as Remarks, " +
                                  //$"oo.product_id as ProductId, " +
                                  $"oo.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"oo.order_date as OrderDate, " +
                                  $"oo.expected_date as ExpectedDate, oo.delivery_date as DeliveryDate, " +
                                  $"oo.reciver_name as ReciverName, oo.order_status as OrderStatus, " +
                                  $"oo.invoice_id as InvoiceId, " +
                                  $"oo.created_by as CreatedBy, oo.modified_by as ModifiedBy";

                var selQuery = $"select " + ColumAssign + " from " + tableName;

                var whereCondi = $"where oo.company_id = co.company_id ";

                if(!string.IsNullOrEmpty(orderId))
                    whereCondi += " and oo.order_id = '" + orderId + "'";

                if(!string.IsNullOrEmpty(companyId))
                    whereCondi += " and oo.company_id = '" + companyId + "'";

                var orderCond = $" order by oo.order_name ASC ";

                var sqlSelQuery = selQuery + whereCondi + orderCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = await connection.QueryAsync<OrderDetails>(sqlSelQuery);
                    retOrderDetailsList = sqlResult.ToList();
                    foreach(OrderDetails singleOrderDetails in retOrderDetailsList)
                    {
                        singleOrderDetails.CustomerDetails = new CustomerDetails();
                        List<CustomerDetails> retVal = await _customerRepository.GetCustomerDetails(singleOrderDetails.CustomerId);
                        if(retVal.Count > 0)
                            singleOrderDetails.CustomerDetails = retVal[0];

                        singleOrderDetails.ProductDetailsList = await _productRepository.GetOrderProductDetails(singleOrderDetails.OrderId);
                        if(singleOrderDetails.ProductDetailsList == null)
                            singleOrderDetails.ProductDetailsList = new List<ProductDetails>();
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retOrderDetailsList;
        }
    }
}