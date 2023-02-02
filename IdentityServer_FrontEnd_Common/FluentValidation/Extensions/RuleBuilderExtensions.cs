using FluentValidation;
using IdentityServer_Common.Constants;

namespace IdentityServer_FrontEnd_Common.FluentValidation.Extensions
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, string> UserName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                          .NotEmpty()
                          .MinimumLength(FluentValidationConstants.MinimumLengthUserName)
                          .MaximumLength(FluentValidationConstants.MaximumLengthUserName);

            return options;
        }

        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                          .NotEmpty()
                          .MinimumLength(FluentValidationConstants.MinimumLengthPassword)
                          .MaximumLength(FluentValidationConstants.MaximumLengthPassword);
                          //.Matches(FluentValidationConstants.PasswordRegEx).WithMessage("regex error");

            return options;
        }
    }
}
