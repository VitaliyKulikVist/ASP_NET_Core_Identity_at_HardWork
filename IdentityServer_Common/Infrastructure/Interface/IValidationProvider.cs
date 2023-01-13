using FluentValidation;

namespace IdentityServer_Common.Infrastructure.Interface
{
    public interface IValidationProvider
    {
        IValidator<TModel> GetValidator<TModel>();
    }
}
