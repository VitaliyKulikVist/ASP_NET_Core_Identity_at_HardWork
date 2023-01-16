using FluentValidation;
using IdentityServer_DAL.Entity.ViewModel.Auth;
using IdentityServer_DAL.FluentValidation.Extensions;

namespace IdentityServer_DAL.FluentValidation.Auth
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
