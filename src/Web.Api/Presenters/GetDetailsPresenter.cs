using System.Net;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Serialization;

namespace Web.Api.Presenters
{
    public sealed class GetDetailsPresenter : IOutputPort<GetDetailsResponse>
    {
        public JsonContentResult ContentResult { get; }

        public GetDetailsPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(GetDetailsResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            ContentResult.Content = response.Success ? JsonSerializer.SerializeObject(response) : JsonSerializer.SerializeObject(response.Errors);
        }
    }
}