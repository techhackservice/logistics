using System.Net;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Serialization;

namespace Web.Api.Presenters
{
    public sealed class AvailabilityPresenter : IOutputPort<AvailabilityResponse>
    {
        public JsonContentResult ContentResult { get; }

        public AvailabilityPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(AvailabilityResponse response)
        {
            ContentResult.StatusCode = (int)(HttpStatusCode.OK);
            ContentResult.Content = JsonSerializer.SerializeObject(response);
        }
    }
}