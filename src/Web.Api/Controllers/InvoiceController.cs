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
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceUseCases _invoiceUseCases;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;
        private readonly GetDetailsPresenter _getDetailsPresenter;
        private readonly AvailabilityPresenter _availabilityPresenter;
        private readonly IMapper _mapper;

        public InvoiceController(IInvoiceUseCases invoiceUseCases, AcknowledgementPresenter acknowledgementPresenter, GetDetailsPresenter getDetailsPresenter, AvailabilityPresenter availabilityPresenter, IMapper mapper)
        {
            _invoiceUseCases = invoiceUseCases;
            _acknowledgementPresenter = acknowledgementPresenter;
            _getDetailsPresenter = getDetailsPresenter;
            _availabilityPresenter = availabilityPresenter;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting a Invoice Details
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <param name="invoiceId">Invoice Id (optional)</param>
        /// <param name="invoiceType">Invoice Type (receivable, payable)</param>
        /// <param name="invoiceStatus">Invoice Status (open, close)</param>
        /// <param name="invoiceFilterStatus">Invoice Filter Status (all, shipping, pickup, shipped, intransit, outofdelivery, delivered, hold, return)</param>
        /// <returns>Invoice Details</returns>
        [HttpGet("company/{companyId}/invoice")]
        public async Task<ActionResult> GetTrackerDetails(string companyId, string invoiceId = "", string invoiceType = "receivable", string invoiceStatus = "open", string invoiceFilterStatus = "all")
        {
            await _invoiceUseCases.Handle(new GetDetailsRequest(companyId, invoiceId, invoiceType, invoiceStatus, invoiceFilterStatus), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Getting the Invoice No
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <param name="invoiceType">Invoice Type (receivable, payable)</param>
        /// <returns>Newly Created Invoice No</returns>
        [HttpGet("company/{companyId}/generate-invoice-no")]
        public async Task<ActionResult> GetInvoiceNo(string companyId, string invoiceType = "receivable")
        {
            await _invoiceUseCases.Handle(new GetDetailsRequest(companyId, true, invoiceType), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Invoice Number Availability
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <param name="invoiceNo">Invoice Number</param>
        /// <param name="invoiceType">Invoice Type (receivable, payable)</param>
        /// <returns></returns>
        [HttpGet("company/{companyId}/invoice-no-available")]
        public async Task<ActionResult> CompanyNameAvailable(string companyId, string invoiceNo, string invoiceType = "receivable")
        {
            await _invoiceUseCases.Handle(new AvailabilityRequest(companyId, invoiceNo, invoiceType, true), _availabilityPresenter);
            return _availabilityPresenter.ContentResult;
        }

        [HttpPost("company/{companyId}/invoice")]
        public async Task<ActionResult> CreateInvoice([FromBody] Models.Request.InvoiceRequest request)
        {
            request.IsUpdate = false;
            await _invoiceUseCases.Handle(_mapper.Map<InvoiceRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }
    }
}