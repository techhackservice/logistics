using System.Collections.Generic;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class LoginResponse : UseCaseResponseMessage
    {
        public IEnumerable<Error> Errors { get; }

        public LoginResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
            Success = success;
            Message = message;
        }

        public LoginResponse(bool success, string message = null) : base(success, message)
        {
            Success = success;
            Message = message;
        }
       
    }
}
