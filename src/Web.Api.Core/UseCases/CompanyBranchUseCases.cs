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
    public sealed class CompanyBranchUseCases : ICompanyBranchUseCases
    {
        private readonly ICompanyBranchRepository _companyBranchRepository;
        public CompanyBranchUseCases(ICompanyBranchRepository companyBranchRepository)
        {
            _companyBranchRepository = companyBranchRepository;
        }

        public async Task<bool> Handle(GetDetailsRequest request, IOutputPort<GetDetailsResponse> outputPort)
        {
            GetDetailsResponse getDetailsResponse;
            getDetailsResponse = new GetDetailsResponse(await _companyBranchRepository.GetCompanyBranchDetails(request.Id, request.CompanyId), true, "Data Fetched Successfully");
            outputPort.Handle(getDetailsResponse);
            return true;
        }
        public async Task<bool> Handle(CompanyBranchRequest companyBranchRequest, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            if(await _companyBranchRepository.CheckCompanyBranchNameAvailability(companyBranchRequest.BranchId, companyBranchRequest.CompanyId, companyBranchRequest.BranchName))
                acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Already Exist", "The Company Branch Name already Available.")}, false);
            else
            {
                if(companyBranchRequest.IsUpdate)//Edit a Company Branch
                {
                    if(await _companyBranchRepository.UpdateCompanyBranch(companyBranchRequest))
                        acknowledgementResponse = new AcknowledgementResponse(companyBranchRequest.CompanyId, true, "Company Branch Successfully Modifyed");
                    else
                        acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", companyBranchRequest.ErrorMsg)}, false);
                }
                else//Create a Company Branch
                {
                    if(await _companyBranchRepository.CreateCompanyBranch(companyBranchRequest))
                        acknowledgementResponse = new AcknowledgementResponse(companyBranchRequest.BranchId, true, "Company Branch Created Successfully");
                    else
                        acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", companyBranchRequest.ErrorMsg)}, false);
                }
            }
            outputPort.Handle(acknowledgementResponse);
            return true;
        }
        public async Task<bool> Handle(DeleteRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;
            if (await _companyBranchRepository.DeleteCompanyBranch(request))
                acknowledgementResponse = new AcknowledgementResponse(true, "Company Branch Deleted Successfully");
            else
                acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", request.ErrorMsg) }, false);
            outputPort.Handle(acknowledgementResponse);
            return true;
        }
        public async Task<bool> Handle(AvailabilityRequest request, IOutputPort<AvailabilityResponse> outputPort)
        {
            AvailabilityResponse availabilityResponse;
            bool retVal = false;
            retVal = await _companyBranchRepository.CheckCompanyBranchNameAvailability(request.Id, request.CompanyId, request.Name);
            availabilityResponse = new AvailabilityResponse(retVal, "Company Branch Name", true);
            outputPort.Handle(availabilityResponse);
            return true;
        }

    }
}