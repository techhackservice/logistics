using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.UseCases
{
    public partial interface IProductUseCases : IUseCaseRequestHandler<GetDetailsRequest, GetDetailsResponse>
    {   }
}