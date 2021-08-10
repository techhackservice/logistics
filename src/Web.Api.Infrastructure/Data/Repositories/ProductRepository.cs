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
    internal sealed class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpClientService _httpClientService;
        public ProductRepository(AppDbContext appDbContext, IHttpClientService httpClientService)
        {
            _appDbContext = appDbContext;
            _httpClientService = httpClientService;
        }
        public async Task<List<ProductDetails>> GetOrderProductDetails(string orderId)
        {
            List<ProductDetails> retProductDetailsList = new List<ProductDetails>();
            try
            {
                var tableName = $"Logisticmate.order_product_xw opxw, " +
                                //$"Logisticmate.order_obj oo, " +
                                $"Logisticmate.product_obj po ";

                var ColumAssign = $"po.product_id as ProductId, po.product_name as ProductName, " + 
                                  $"po.price as Price, po.discount as Discount, " +
                                  $"po.total_amt as SingleProductAmount, " +
                                  $"po.gst_percentage as GSTPercentage, " +
                                  $"opxw.no_of_items as ItemCount, opxw.size as Size, " +
                                  $"opxw.weight as Weight, opxw.total_amt as TotalAmount, " +
                                  $"opxw.created_by as CreatedBy, opxw.modified_by as ModifiedBy";

                var selQuery = $"select " + ColumAssign + " from " + tableName;

                var whereCondi = $"where opxw.product_id = po.product_id";

                if(!string.IsNullOrEmpty(orderId))
                    whereCondi += " and opxw.order_id = '" + orderId + "'";

                var orderCond = $" order by po.product_name ASC ";

                var sqlSelQuery = selQuery + whereCondi + orderCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = await connection.QueryAsync<ProductDetails>(sqlSelQuery);
                    retProductDetailsList = sqlResult.ToList();
                }
            }
            catch(Exception Err)
            {var Error = Err.Message.ToString();}
            return retProductDetailsList;
        }
        /*public async Task<List<ProductDetails>> No_GetProductDetails(string productId)
        {
            List<ProductDetails> retProductDetailsList = new List<ProductDetails>();

            try
            {
                var tableName = $"Logisticmate.product_obj po ";

                var ColumAssign = $"po.product_id as ProductId, po.product_name as ProductName, " + 
                                  $"po.price as Price, po.discount as Discount, " +
                                  $"po.no_of_items as ItemCount, po.size as Size, " +
                                  $"po.weight as Weight, po.total_amt as TotalAmount, " +
                                  $"po.created_by as CreatedBy, po.modified_by as ModifiedBy";

                var selQuery = $"select " + ColumAssign + " from " + tableName;

                var whereCondi = $"";

                if(!string.IsNullOrEmpty(productId) && String.IsNullOrEmpty(whereCondi))
                    whereCondi += "po.product_id = '" + productId + "'";
                else if(!string.IsNullOrEmpty(productId) && !String.IsNullOrEmpty(whereCondi))
                    whereCondi += " and po.product_id = '" + productId + "'";

                if(!string.IsNullOrEmpty(whereCondi))
                    whereCondi = "where " + whereCondi;

                var orderCond = $" order by po.product_name ASC ";

                var sqlSelQuery = selQuery + whereCondi + orderCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = await connection.QueryAsync<ProductDetails>(sqlSelQuery);
                    retProductDetailsList = sqlResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retProductDetailsList;
        }*/
    }
}