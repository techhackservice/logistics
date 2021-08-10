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
    public class TrackerController : ControllerBase
    {
        private readonly ITrackerUseCases _trackerUseCases;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;
        private readonly GetDetailsPresenter _getDetailsPresenter;
        private readonly IMapper _mapper;

        public TrackerController(ITrackerUseCases trackerUseCases, AcknowledgementPresenter acknowledgementPresenter, GetDetailsPresenter getDetailsPresenter, AvailabilityPresenter availabilityPresenter, IMapper mapper)
        {
            _trackerUseCases = trackerUseCases;
            _acknowledgementPresenter = acknowledgementPresenter;
            _getDetailsPresenter = getDetailsPresenter;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting a Tracking Details
        /// </summary>
        /// <param name="trackerId">Track Id </param>
        /// <param name="email">Email (optional)</param>
        /// <param name="companyUrl">Company Url (optional)</param>
        /// <returns>Trcking Details</returns>
        [HttpGet("tracker")]
        public async Task<ActionResult> GetTrackerDetails(string trackerId = "", string email = "", string companyUrl = "")
        {
            await _trackerUseCases.Handle(new GetDetailsRequest(trackerId, email, companyUrl), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }
    }
}