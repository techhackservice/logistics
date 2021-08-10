using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Presenters;
using AutoMapper;


namespace Web.Api.Controllers
{
    [Route("v1")]
    [ApiController]
    public class CompanyBranchController : ControllerBase
    {
        private readonly ICompanyBranchUseCases _companyBranchUseCases;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;
        private readonly GetDetailsPresenter _getDetailsPresenter;
        private readonly AvailabilityPresenter _availabilityPresenter;
        private readonly IMapper _mapper;

        public CompanyBranchController(ICompanyBranchUseCases companyBranchUseCases, AcknowledgementPresenter acknowledgementPresenter, GetDetailsPresenter getDetailsPresenter, AvailabilityPresenter availabilityPresenter, IMapper mapper)
        {   
            _companyBranchUseCases = companyBranchUseCases;
            _acknowledgementPresenter = acknowledgementPresenter;
            _getDetailsPresenter = getDetailsPresenter;
            _availabilityPresenter = availabilityPresenter;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting a Company Branch Details
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <param name="branchId">Branch Id (optional)</param>
        /// <returns>List of Company Branch Details</returns>
        [HttpGet("company/{companyId}/company-branch")]
        public async Task<ActionResult> GetCompanyBranchDetails(string companyId, string branchId = "")
        {
            await _companyBranchUseCases.Handle(new GetDetailsRequest(branchId, companyId, false), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Creating a Company Branch
        /// </summary>
        /// <param name="request">New Company Branch Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPost("company/{companyId}/company-branch")]
        public async Task<ActionResult> CreateCompanyBranch([FromBody] Models.Request.CompanyBranchRequest request)
        {
            request.IsUpdate = false;
            await _companyBranchUseCases.Handle(_mapper.Map<CompanyBranchRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Modifying a Company Branch
        /// </summary>
        /// <param name="request">Modifying a Company Branch Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("company/{companyId}/company-branch")]
        public async Task<ActionResult> UpdateCompanyBranch([FromBody] Models.Request.CompanyBranchRequest request)
        {
            request.IsUpdate = true;
            await _companyBranchUseCases.Handle(_mapper.Map<CompanyBranchRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Deleting a Company Branch
        /// </summary>
        /// <param name="request">Delete Company Branch Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpDelete("company/{companyId}/company-branch")]
        public async Task<ActionResult> DeleteCompanyBranch([FromBody] Models.Request.DeleteRequest request)
        {
            await _companyBranchUseCases.Handle(_mapper.Map<DeleteRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Company Branch Name Availability
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <param name="branchName">Branch Name</param>
        /// <param name="branchId">Branch Id (Optional)</param>
        /// <returns></returns>
        [HttpGet("company/{companyId}/branch-name-available")]
        public async Task<ActionResult> CompanyBranchNameAvailable(string companyId, string branchName, string branchId = "")
        {
            await _companyBranchUseCases.Handle(new AvailabilityRequest(companyId, branchId, branchName), _availabilityPresenter);
            return _availabilityPresenter.ContentResult;
        }
    }
}