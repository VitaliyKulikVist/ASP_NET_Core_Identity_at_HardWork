using FluentValidation;
using IdentityServer_Common.Infrastructure.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IdentityServer_Common.Providers
{
    public class ValidationProvider : IValidationProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationProvider (IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider 
                ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IValidator<TModel> GetValidator<TModel>()
        {
            return _serviceProvider.GetRequiredService<IValidator<TModel>>();
        }
    }
}
