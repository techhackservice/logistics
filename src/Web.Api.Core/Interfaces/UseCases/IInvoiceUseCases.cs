using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.UseCases
{
    public partial interface IInvoiceUseCases : IUseCaseRequestHandler<GetDetailsRequest, GetDetailsResponse>
    {   }
    public partial interface IInvoiceUseCases : IUseCaseRequestHandler<AvailabilityRequest, AvailabilityResponse>
    {   }
    public partial interface IInvoiceUseCases : IUseCaseRequestHandler<InvoiceRequest, AcknowledgementResponse>
    {   }
}