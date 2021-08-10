using Moq;
using System.Net;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.UseCases;
using Web.Api.Presenters;
using Xunit;

namespace Web.Api.Core.UnitTests.UseCases
{
    public class LoginUseCaseUnitTests
    {
        public readonly string _userName = "test@admin.com";
        public readonly string _password = "test";
     
        /// <summary>
        /// Username present, password is correct and Account is not inactive 
        /// </summary>
        [Fact]
        public async void Handle_AllCorrectDetailsActiveAccount()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.CheckUsername(_userName)).ReturnsAsync(true);
          
            var useCase = new AuthUseCases(mockUserRepository.Object);
            var userDetailsSuccess = new LoginPresenter();
            await useCase.Handle(new LoginRequest(_userName, _password), userDetailsSuccess);

            Assert.Equal((int)HttpStatusCode.OK, userDetailsSuccess.ContentResult.StatusCode);
        }

    }
}
