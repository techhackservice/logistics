using System;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class TrackerDetails : IUseCaseRequest<AcknowledgementResponse>
    {
        public string TrackingId { get; set; }
        public string OrderId { get; set; }
        public OrderDetails OrderDetails { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public DateTime ReturnDate { get; set; }
        public List<TrackerHistoryDetails> TrackerHistoryDetailsList { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class TrackerHistoryDetails : IUseCaseRequest<AcknowledgementResponse>
    {
        public string TrackingId { get; set; }
        public string OrderId { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string CreatedBy { get; set; }
    }
}