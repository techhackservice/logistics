using FluentValidation;
using Web.Api.Models.Request;

namespace Web.Api.Models.Validation
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().Matches(@"\A\S+\z").NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
        }
    }
}
