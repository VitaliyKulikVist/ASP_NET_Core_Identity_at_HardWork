using IdentityServer_DAL_MySQL.MenegmentData;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IdentityServer.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegistrationDI(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<DeleteData>();
            services.AddTransient<SeedData>();

            return services;
        }
    }
}
