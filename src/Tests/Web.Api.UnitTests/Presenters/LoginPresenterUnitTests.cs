using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Presenters;
using Xunit;

namespace Web.Api.UnitTests.Presenters
{
    public class LoginPresenterUnitTests
    {
        /// <summary>
        /// Failed response throws errors
        /// </summary>
        [Fact]
        public void Handle_FailedResponse()
        {
            var presenter = new LoginPresenter();

            presenter.Handle(new LoginResponse(new[] { new Error("", "Invalid username/password") }));

            Assert.Equal((int)HttpStatusCode.BadRequest, presenter.ContentResult.StatusCode);
        }
    }
}
