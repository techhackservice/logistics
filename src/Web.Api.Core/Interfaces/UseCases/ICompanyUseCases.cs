using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.UseCases
{
    public partial interface ICompanyUseCases : IUseCaseRequestHandler<GetDetailsRequest, GetDetailsResponse>
    {   }
    public partial interface ICompanyUseCases : IUseCaseRequestHandler<CompanyRequest, AcknowledgementResponse>
    {   }
    public partial interface ICompanyUseCases : IUseCaseRequestHandler<DeleteRequest, AcknowledgementResponse>
    {   }
    public partial interface ICompanyUseCases : IUseCaseRequestHandler<AvailabilityRequest, AvailabilityResponse>
    {   }
    public partial interface ICompanyUseCases : IUseCaseRequestHandler<ConvinienceFeesDetails, AcknowledgementResponse>
    {   }
}