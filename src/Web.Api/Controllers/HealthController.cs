using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Presenters;

namespace Web.Api.Controllers
{
    [Route("v1")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IHealthUseCases _healthUseCases;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;

        public HealthController(IHealthUseCases healthUseCases, AcknowledgementPresenter acknowledgementPresenter)
        {
            _healthUseCases = healthUseCases;
            _acknowledgementPresenter = acknowledgementPresenter;
        }

        /// <summary>
        /// Health Check
        /// </summary>
        /// <returns>User details with Token</returns>
        [HttpGet("health")]
        public async Task<ActionResult> Health()
        {
            await _healthUseCases.Handle(null, _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }
    }
}