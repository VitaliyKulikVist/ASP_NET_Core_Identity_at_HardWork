using FluentValidation;
using IdentityServer_FrontEnd_Common.Entity.ViewModel.Auth;
using IdentityServer_FrontEnd_Common.FluentValidation.Extensions;

namespace IdentityServer_FrontEnd_Common.FluentValidation.Auth
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel> 
    {
        public LoginViewModelValidator() 
        {
            RuleFor(x => x.UserName).UserName();
            RuleFor(x => x.Password).Password();
        }
    }
}
