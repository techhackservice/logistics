using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class GetDetailsResponse : UseCaseResponseMessage
    {
        public dynamic Details { get; set; }
        public IEnumerable<Error> Errors { get; }

        public GetDetailsResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
            Success = success;
            Message = message;
        }

        public GetDetailsResponse(dynamic details ,bool success, string message = null) : base(success, message)
        {
            Details = details;
            Success = success;
            Message = message;
        }
    }
}