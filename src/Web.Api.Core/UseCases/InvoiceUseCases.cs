using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.UseCases;


namespace Web.Api.Core.UseCases
{
    public sealed class InvoiceUseCases : IInvoiceUseCases
    {
        private readonly IInvoiceRepository _invoiceRepository;
        public InvoiceUseCases(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<bool> Handle(GetDetailsRequest request, IOutputPort<GetDetailsResponse> outputPort)
        {
            GetDetailsResponse getDetailsResponse;
            if(request.IsGetInvoiceNo)
                getDetailsResponse = new GetDetailsResponse(await _invoiceRepository.GenerateInvoiceNo(request.CompanyId, request.InvoiceType), true, "Data Fetched Successfully");
            else
                getDetailsResponse = new GetDetailsResponse(await _invoiceRepository.GetInvoiceDetails(request.CompanyId, request.InvoiceId, request.InvoiceType, request.InvoiceStatus, request.InvoiceFilterStatus), true, "Data Fetched Successfully");
            outputPort.Handle(getDetailsResponse);
            return true;
        }
        public async Task<bool> Handle(AvailabilityRequest request, IOutputPort<AvailabilityResponse> outputPort)
        {
            AvailabilityResponse availabilityResponse;
            bool retVal = false;
            retVal = await _invoiceRepository.CheckInvoiceNoAvailability(request.CompanyId, request.InvoiceType, request.InvoiceNo);
            availabilityResponse = new AvailabilityResponse(retVal, "Invoice Number", true);
            outputPort.Handle(availabilityResponse);
            return true;
        }
        public async Task<bool> Handle(InvoiceRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            if(await _invoiceRepository.CheckInvoiceNoAvailability(request.CompanyId, request.InvoiceType, request.InvoiceId))
                acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Already Exist", "The Invoice Number already Available.")}, false);
            else
            {
                /*if(request.IsUpdate)//Edit a Invoice
                {
                    if(await _invoiceRepository.UpdateInvoice(request))
                        acknowledgementResponse = new AcknowledgementResponse(request.CompanyId, true, "Company Successfully Modifyed");
                    else
                        acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", request.ErrorMsg)}, false);
                }
                else//Create a Invoice*/
                {
                    if(await _invoiceRepository.CreateInvoice(request))
                        acknowledgementResponse = new AcknowledgementResponse(request.CompanyId, true, "Company Created Successfully");
                    else
                        acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", request.ErrorMsg)}, false);
                }
            }
            outputPort.Handle(acknowledgementResponse);
            return true;
        }

    }
}