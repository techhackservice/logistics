using System.Net;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Serialization;

namespace Web.Api.Presenters
{
    public sealed class AcknowledgementPresenter : IOutputPort<AcknowledgementResponse>
    {
        public JsonContentResult ContentResult { get; }

        public AcknowledgementPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(AcknowledgementResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            ContentResult.Content = response.Success ? JsonSerializer.SerializeObject(response) : JsonSerializer.SerializeObject(response.Errors);
        }
    }
}
