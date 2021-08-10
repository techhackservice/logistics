using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class DeleteRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public string Id { get; }
        public string DeletedBy { get; }
        public string ErrorMsg { get; set; }

        public DeleteRequest(string id, string deletedBy)
        {
            Id = id;
            DeletedBy = deletedBy;
        }
    }
}