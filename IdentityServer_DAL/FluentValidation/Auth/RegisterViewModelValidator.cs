﻿using FluentValidation;
using IdentityServer_DAL.Entity.Auth;
using IdentityServer_DAL.FluentValidation.Extensions;

namespace IdentityServer_DAL.FluentValidation.Auth
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator() 
        {
            RuleFor(x => x.UserName).UserName().WithMessage("Please specify a User Name.");
            RuleFor(x => x.Password).Password().WithMessage("Please specify a Password.");
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(customer => customer.Password).Must(BeAValidConfirmPassword).WithMessage("Please confirm a Password."); ;
            RuleFor(x => x.ReturnUrl);
        }

        private bool BeAValidConfirmPassword(string confirmPassword)
        {
            // custom confirmPassword validating logic goes here.


            return true;
        }
    }
}