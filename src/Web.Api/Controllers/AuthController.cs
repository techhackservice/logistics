using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Models.Settings;
using Web.Api.Presenters;

namespace Web.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthUseCases _authUseCases;
        private readonly LoginPresenter _loginPresenter;

        public AuthController(IAuthUseCases authUseCases, LoginPresenter loginPresenter)
        {   
            _authUseCases = authUseCases;
            _loginPresenter = loginPresenter;
        }

        /// <summary>
        /// Login request
        /// </summary>
        /// <param name="request">Username and password should be given</param>
        /// <returns>User details with Token</returns>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] Models.Request.LoginRequest request)
        {
            await _authUseCases.Handle(new LoginRequest(request.UserName, request.Password), _loginPresenter);
            return _loginPresenter.ContentResult;
        }

    }
}
