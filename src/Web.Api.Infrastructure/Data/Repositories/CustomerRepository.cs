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
    internal sealed class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpClientService _httpClientService;
        public CustomerRepository(AppDbContext appDbContext, IHttpClientService httpClientService)
        {
            _appDbContext = appDbContext;
            _httpClientService = httpClientService;
        }
        public async Task<List<CustomerDetails>> GetCustomerDetails(string customerId)
        {
            List<CustomerDetails> retCustomerDetailsList = new List<CustomerDetails>();

            try
            {/*customer_id,customer_first_name
customer_middle_name,customer_last_name,
shipping_address_id,delivery_address_id,status,
created_by,created_on,modified_by,modified_on
            */
                var tableName = $"Logisticmate.customer_obj co ";

                var ColumAssign = $"co.customer_id as CustomerId, co.customer_first_name as FirstName, " + 
                                  $"co.customer_middle_name as MiddleName, co.customer_last_name as LastName, " +
                                  $"co.shipping_address_id as ShippingAddressId, co.delivery_address_id as DeliveryAddressId, " +
                                  $"co.status as Status, " +
                                  $"co.created_by as CreatedBy, co.modified_by as ModifiedBy";

                var selQuery = $"select " + ColumAssign + " from " + tableName;

                var whereCondi = $"";

                if(!string.IsNullOrEmpty(customerId) && String.IsNullOrEmpty(whereCondi))
                    whereCondi += "co.customer_id = '" + customerId + "'";
                else if(!string.IsNullOrEmpty(customerId) && !String.IsNullOrEmpty(whereCondi))
                    whereCondi += " and co.customer_id = '" + customerId + "'";

                var activeCond = "";
                if(String.IsNullOrEmpty(whereCondi))
                    activeCond = $"where co.status = " + (int)Status.Active;
                else
                    activeCond = $" and co.status = " + (int)Status.Active;

                if(!string.IsNullOrEmpty(whereCondi))
                    whereCondi = "where " + whereCondi;

                var orderCond = $" order by co.modified_by ASC ";

                var sqlSelQuery = selQuery + whereCondi + activeCond + orderCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = await connection.QueryAsync<CustomerDetails>(sqlSelQuery);
                    retCustomerDetailsList = sqlResult.ToList();
                    foreach(CustomerDetails singleCustomerDetails in retCustomerDetailsList)
                    {
                        singleCustomerDetails.ShippingAddressDetails = await GetAddressDetails(singleCustomerDetails.ShippingAddressId);
                        singleCustomerDetails.DeliveryAddressDetails = await GetAddressDetails(singleCustomerDetails.DeliveryAddressId);
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retCustomerDetailsList;
        }
        public async Task<AddressDetails> GetAddressDetails(string addressId)
        {
           AddressDetails retAddressDetails = new AddressDetails();

            try
            {/*address_id,first_line
second_line,city,state,country,zipcode,land_mark
google_map_link,std_code_landline_number,landline_number,std_code_mobile_number
mobile_number,email,status,created_by,created_on,modified_by,modified_on
            */
                var tableName = $"Logisticmate.address_obj ao ";

                var ColumAssign = $"ao.address_id as AddressId, ao.first_line as FirstLine, " + 
                                  $"ao.second_line as SecondLine, ao.city as City, " +
                                  $"ao.state as State, ao.country as Country, " +
                                  $"ao.zipcode as Zipcode, ao.land_mark as Landmark, " +
                                  $"ao.google_map_link as GoogleMapLink, " +
                                  $"ao.std_code_landline_number as STDCodeLandlineNumber, " +
                                  $"ao.landline_number as LandlineNumber, " +
                                  $"ao.std_code_mobile_number as STDCodeMobileNumber, " +
                                  $"ao.mobile_number as MobileNumber, ao.email as Email, " +
                                  $"ao.status as Status, " +
                                  $"ao.created_by as CreatedBy, ao.modified_by as ModifiedBy";

                var selQuery = $"select " + ColumAssign + " from " + tableName;

                var whereCondi = $"";

                if(!string.IsNullOrEmpty(addressId) && String.IsNullOrEmpty(whereCondi))
                    whereCondi += "ao.address_id = '" + addressId + "'";
                else if(!string.IsNullOrEmpty(addressId) && !String.IsNullOrEmpty(whereCondi))
                    whereCondi += " and ao.address_id = '" + addressId + "'";

                var activeCond = "";
                if(String.IsNullOrEmpty(whereCondi))
                    activeCond = $"where ao.status = '" + Status.Active.ToString() + "'";
                else
                    activeCond = $" and ao.status = '" + Status.Active.ToString() + "'";

                if(!string.IsNullOrEmpty(whereCondi))
                    whereCondi = "where " + whereCondi;

                var orderCond = $" order by ao.modified_by ASC ";

                var sqlSelQuery = selQuery + whereCondi + activeCond + orderCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = await connection.QueryAsync<AddressDetails>(sqlSelQuery);
                    retAddressDetails = sqlResult.FirstOrDefault();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                retAddressDetails = new AddressDetails();
                retAddressDetails.AddressId = addressId;
            }
            return retAddressDetails;
        }
    }
}