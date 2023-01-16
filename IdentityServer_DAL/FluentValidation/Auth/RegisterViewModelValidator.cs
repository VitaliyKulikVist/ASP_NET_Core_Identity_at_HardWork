using FluentValidation;
using IdentityServer_DAL.Entity.ViewModel.Auth;
using IdentityServer_DAL.FluentValidation.Extensions;

namespace IdentityServer_DAL.FluentValidation.Auth
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator() 
        {
            RuleFor(x => x.UserName)
                .UserName()
                .WithMessage("Please specify a User Name.");
            RuleFor(x => x.Password)
                .Password()
                .WithMessage("Please specify a Password.");
            RuleFor(x => x.ConfirmPassword)
                .Password()
                .Equal(customer => customer.Password)
                .WithMessage("Please confirm a Password.");
        }
    }
}
