using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class AvailabilityResponse : UseCaseResponseMessage
    {
        public bool IsAvailable { get; set; }
        public string FieldName { get; set; }

        public AvailabilityResponse(bool isAvailable, string fieldName ,bool success, string message = null) : base(success, message)
        {
            IsAvailable = isAvailable;
            FieldName = fieldName;
            Success = success;
            Message = message;
        }

    }
}
