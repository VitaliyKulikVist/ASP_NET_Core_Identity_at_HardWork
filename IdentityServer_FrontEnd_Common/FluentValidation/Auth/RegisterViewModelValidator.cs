using FluentValidation;
using IdentityServer_FrontEnd_Common.Entity.ViewModel.Auth;
using IdentityServer_FrontEnd_Common.FluentValidation.Extensions;

namespace IdentityServer_FrontEnd_Common.FluentValidation.Auth
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator() 
        {
            RuleFor(x => x.UserName)
                .UserName();
            RuleFor(x => x.Password)
                .Password();
            RuleFor(x => x.ConfirmPassword)
                .Password()
                .Equal(customer => customer.Password);
        }
    }
}
