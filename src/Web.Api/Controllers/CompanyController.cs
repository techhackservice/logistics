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
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyUseCases _companyUseCases;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;
        private readonly GetDetailsPresenter _getDetailsPresenter;
        private readonly AvailabilityPresenter _availabilityPresenter;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyUseCases companyUseCases, AcknowledgementPresenter acknowledgementPresenter, GetDetailsPresenter getDetailsPresenter, AvailabilityPresenter availabilityPresenter, IMapper mapper)
        {   
            _companyUseCases = companyUseCases;
            _acknowledgementPresenter = acknowledgementPresenter;
            _getDetailsPresenter = getDetailsPresenter;
            _availabilityPresenter = availabilityPresenter;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting a Company Details
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="companyId">Company Id (optional)</param>
        /// <returns>List of Company Details</returns>
        [HttpGet("tenants/{tenantId}/company")]
        public async Task<ActionResult> GetCompanyDetails( string tenantId, string companyId = "")
        {
            await _companyUseCases.Handle(new GetDetailsRequest(companyId, tenantId), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Creating a Company
        /// </summary>
        /// <param name="request">New Company Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPost("tenants/{tenantId}/company")]
        public async Task<ActionResult> CreateCompany([FromBody] Models.Request.CompanyRequest request)
        {
            request.IsUpdate = false;
            await _companyUseCases.Handle(_mapper.Map<CompanyRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Modifying a Company
        /// </summary>
        /// <param name="request">Modifying a Company Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("tenants/{tenantId}/company")]
        public async Task<ActionResult> UpdateCompany([FromBody] Models.Request.CompanyRequest request)
        {
            request.IsUpdate = true;
            await _companyUseCases.Handle(_mapper.Map<CompanyRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Deleting a Company
        /// </summary>
        /// <param name="request">Delete Company Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpDelete("tenants/{tenantId}/company")]
        public async Task<ActionResult> DeleteCompany([FromBody] Models.Request.DeleteRequest request)
        {
            await _companyUseCases.Handle(_mapper.Map<DeleteRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Company Name Availability
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="companyName">Company Name</param>
        /// <param name="companyId">Company Id (Optional)</param>
        /// <returns></returns>
        [HttpGet("tenants/{tenantId}/company-name-available")]
        public async Task<ActionResult> CompanyNameAvailable(string tenantId, string companyName, string companyId = "")
        {
            await _companyUseCases.Handle(new AvailabilityRequest(companyId, tenantId, companyName), _availabilityPresenter);
            return _availabilityPresenter.ContentResult;
        }

        /// <summary>
        /// Getting a Convinience Fees Details
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <returns>List of Convinience Fees Details</returns>
        [HttpGet("company/{companyId}/convinience-fee")]
        public async Task<ActionResult> GetConvinienceFeesDetails(string companyId)
        {
            await _companyUseCases.Handle(new GetDetailsRequest(companyId, true), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Modify a Convinience Fees
        /// </summary>
        /// <param name="request">Convinience Fees Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("company/{companyId}/convinience-fee")]
        public async Task<ActionResult> UpdateConvinienceFees([FromBody] Models.Request.ConvinienceFeesDetails request)
        {
            await _companyUseCases.Handle(_mapper.Map<ConvinienceFeesDetails>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }
    }
}