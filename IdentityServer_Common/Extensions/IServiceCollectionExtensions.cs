using IdentityServer_Common.Infrastructure.Interface;
using IdentityServer_Common.Providers;
using IdentityServer_Common.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IdentityServer_Common.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IValidation, Validation>();
            services.AddTransient<IValidationProvider, ValidationProvider>();

            return services;
        }
    }
}
