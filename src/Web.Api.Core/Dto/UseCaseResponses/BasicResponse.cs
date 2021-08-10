using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class BasicResponse : UseCaseResponseMessage
    {
        public IEnumerable<Error> Errors { get; }

        public BasicResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
            Success = success;
            Message = message;
        }

        public BasicResponse(bool success, string message = null) : base(success, message)
        {
            Success = success;
            Message = message;
        }

    }
}
