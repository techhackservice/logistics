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
    public sealed class AuthUseCases : IAuthUseCases
    {
        private readonly IUserRepository _userRepository;
       
        public AuthUseCases(IUserRepository userRepository)
        {
            _userRepository = userRepository;
           
        }

        /// <summary>
        /// Login check
        /// </summary>
        /// <param name="message"></param>
        /// <param name="outputPort"></param>
        /// <returns></returns>
        public async Task<bool> Handle(LoginRequest message, IOutputPort<LoginResponse> outputPort)
        {
            LoginResponse loginResponse;

          
                if ( !await _userRepository.CheckUsername(message.UserName))
                {
                    loginResponse = new LoginResponse(new[] { new Error("login_failure", "Invalid Request", 25 )}, false);
                }
                else
                {
                    loginResponse = new LoginResponse(true, "Login Successful");
                }
            outputPort.Handle(loginResponse);
            return true;
        } 

    }
}
