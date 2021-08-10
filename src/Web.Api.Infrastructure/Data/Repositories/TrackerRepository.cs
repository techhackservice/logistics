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
    internal sealed class TrackerRepository : ITrackerRepository
    {
        private IOrderRepository _orderRepository;
        private readonly AppDbContext _appDbContext;
        private readonly IHttpClientService _httpClientService;
        public TrackerRepository(IOrderRepository orderRepository, AppDbContext appDbContext, IHttpClientService httpClientService)
        {
            _orderRepository = orderRepository;
            _appDbContext = appDbContext;
            _httpClientService = httpClientService;
        }
        public async Task<List<TrackerDetails>> GetTrackerDetails(string trackId, string email, string companyUrl)
        {
            List<TrackerDetails> retTrackerDetailsList = new List<TrackerDetails>();

            try
            {
                List<string> trackIdList = trackId.Split(',').ToList();
                //foreach(string singleTrackId in trackIdList)
                //{
                var tableName = $"Logisticmate.tracking_obj t ";

                var ColumAssign = $"t.tracking_id as TrackingId, t.order_id as OrderId, " + 
                                    $"t.tracking_status as Status, t.tracking_remarks as Remarks, " +
                                    $"t.return_date as ReturnDate, " +
                                    $"t.created_by as CreatedBy, t.modified_by as ModifiedBy";

                var selQuery = $"select " + ColumAssign + " from " + tableName;

                var whereCondi = $"";

                //if(!string.IsNullOrEmpty(singleTrackId))
                    //whereCondi += "t.tracking_id = '" + singleTrackId + "'";
                int i = 0;
                foreach(string singleTrackId in trackIdList)
                {
                    if(!string.IsNullOrEmpty(singleTrackId))
                    {
                        if(i == 0)
                            whereCondi += "t.tracking_id = '" + singleTrackId + "'";
                        else
                            whereCondi += " or t.tracking_id = '" + singleTrackId + "'";
                        i+=1;
                    }
                }

                if(!string.IsNullOrEmpty(whereCondi))
                    whereCondi = "where " + whereCondi;

                //var orderCond = $" order by t.tracking_id ASC ";

                var sqlSelQuery = selQuery + whereCondi;// + orderCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = await connection.QueryAsync<TrackerDetails>(sqlSelQuery);
                    foreach(TrackerDetails singleTrackerDetails in sqlResult.ToList())
                    {
                        singleTrackerDetails.OrderDetails = new OrderDetails();
                        List<OrderDetails> retOrderDet = await _orderRepository.GetOrderDetails("", singleTrackerDetails.OrderId);
                        if(retOrderDet.Count > 0)
                            singleTrackerDetails.OrderDetails = retOrderDet[0];

                        singleTrackerDetails.TrackerHistoryDetailsList = new List<TrackerHistoryDetails>();
                        singleTrackerDetails.TrackerHistoryDetailsList = await GetTrackerHistoryDetails(singleTrackerDetails.TrackingId, singleTrackerDetails.OrderId);
                        retTrackerDetailsList.Add(singleTrackerDetails);
                    }
                }
                //}
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retTrackerDetailsList;
        }

        async Task<List<TrackerHistoryDetails>> GetTrackerHistoryDetails(string trackId, string orderId)
        {
            List<TrackerHistoryDetails> retTrackerHistoryDetailsList = new List<TrackerHistoryDetails>();

            try
            {
                var tableName = $"Logisticmate.tracking_history_obj tho ";

                var ColumAssign = $"tho.tracking_id as TrackingId, tho.order_id as OrderId, " + 
                                    $"tho.description as Description, tho.location as Location, " +
                                    $"tho.tracking_status as Status, tho.status_date as StatusDate, " +
                                    $"tho.created_by as CreatedBy";

                var selQuery = $"select " + ColumAssign + " from " + tableName;

                var whereCondi = $"";

                if(!string.IsNullOrEmpty(trackId) && String.IsNullOrEmpty(whereCondi))
                    whereCondi += "tho.tracking_id = '" + trackId + "'";
                else if(!string.IsNullOrEmpty(trackId) && !String.IsNullOrEmpty(whereCondi))
                    whereCondi += " and tho.tracking_id = '" + trackId + "'";

                if(!string.IsNullOrEmpty(orderId) && String.IsNullOrEmpty(whereCondi))
                    whereCondi += "tho.order_id = '" + orderId + "'";
                else if(!string.IsNullOrEmpty(orderId) && !String.IsNullOrEmpty(whereCondi))
                    whereCondi += " and tho.order_id = '" + orderId + "'";

                if(!string.IsNullOrEmpty(whereCondi))
                    whereCondi = "where " + whereCondi;

                var orderCond = $" order by tho.created_by ASC ";

                var sqlSelQuery = selQuery + whereCondi + orderCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = await connection.QueryAsync<TrackerHistoryDetails>(sqlSelQuery);
                    retTrackerHistoryDetailsList = sqlResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retTrackerHistoryDetailsList;
        }

    }
}