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
    public sealed class CompanyUseCases : ICompanyUseCases
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyUseCases(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<bool> Handle(GetDetailsRequest request, IOutputPort<GetDetailsResponse> outputPort)
        {
            GetDetailsResponse getDetailsResponse;
            if(request.IsConvinienceFees)
                getDetailsResponse = new GetDetailsResponse(await _companyRepository.GetConvinienceFeesDetails(request.CompanyId), true, "Data Fetched Successfully");
            else
                getDetailsResponse = new GetDetailsResponse(await _companyRepository.GetCompanyDetails(request.CompanyId, request.TenantId), true, "Data Fetched Successfully");
            outputPort.Handle(getDetailsResponse);
            return true;
        }
        public async Task<bool> Handle(CompanyRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            if(await _companyRepository.CheckCompanyNameAvailability(request.TenantId, request.CompanyId, request.CompanyName))
                acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Already Exist", "The Company Name already Available.")}, false);
            else
            {
                if(request.IsUpdate)//Edit a Company
                {
                    if(await _companyRepository.UpdateCompany(request))
                        acknowledgementResponse = new AcknowledgementResponse(request.CompanyId, true, "Company Successfully Modifyed");
                    else
                        acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", request.ErrorMsg)}, false);
                }
                else//Create a Company
                {
                    if(await _companyRepository.CreateCompany(request))
                        acknowledgementResponse = new AcknowledgementResponse(request.CompanyId, true, "Company Created Successfully");
                    else
                        acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", request.ErrorMsg)}, false);
                }
            }
            outputPort.Handle(acknowledgementResponse);
            return true;
        }
        public async Task<bool> Handle(DeleteRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;
            if (await _companyRepository.DeleteCompany(request))
                acknowledgementResponse = new AcknowledgementResponse(true, "Company Deleted Successfully");
            else
                acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", request.ErrorMsg) }, false);
            outputPort.Handle(acknowledgementResponse);
            return true;
        }
        public async Task<bool> Handle(AvailabilityRequest request, IOutputPort<AvailabilityResponse> outputPort)
        {
            AvailabilityResponse availabilityResponse;
            bool retVal = false;
            retVal = await _companyRepository.CheckCompanyNameAvailability(request.Id, request.CompanyId, request.Name);
            availabilityResponse = new AvailabilityResponse(retVal, "Company Name", true);
            outputPort.Handle(availabilityResponse);
            return true;
        }
        public async Task<bool> Handle(ConvinienceFeesDetails request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            if(await _companyRepository.UpdateConvinienceFees(request))
                acknowledgementResponse = new AcknowledgementResponse(request.CompanyId, true, "Convinience Fees Successfully Modifyed");
            else
                acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", request.ErrorMsg)}, false);
            outputPort.Handle(acknowledgementResponse);
            return true;
        }

    }
}