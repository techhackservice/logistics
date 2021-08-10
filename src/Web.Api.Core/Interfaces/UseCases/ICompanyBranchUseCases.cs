using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.UseCases
{
    public partial interface ICompanyBranchUseCases : IUseCaseRequestHandler<GetDetailsRequest, GetDetailsResponse>
    {   }
    public partial interface ICompanyBranchUseCases : IUseCaseRequestHandler<CompanyBranchRequest, AcknowledgementResponse>
    {   }
    public partial interface ICompanyBranchUseCases : IUseCaseRequestHandler<DeleteRequest, AcknowledgementResponse>
    {   }
    public partial interface ICompanyBranchUseCases : IUseCaseRequestHandler<AvailabilityRequest, AvailabilityResponse>
    {   }
}