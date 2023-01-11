using FluentValidation;
using IdentityServer_DAL.Entity.Auth;
using IdentityServer_DAL.FluentValidation.Extensions;

namespace IdentityServer_DAL.FluentValidation.Auth
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel> 
    {
        public LoginViewModelValidator() 
        {
            RuleFor(x => x.UserName).UserName().WithMessage("Please specify a User Name.");
            RuleFor(x => x.Password).Password().WithMessage("Please specify a Password.");
            RuleFor(x => x.ReturnUrl);
        }
    }
}
