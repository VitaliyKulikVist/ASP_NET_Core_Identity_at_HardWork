using FluentValidation.AspNetCore;
using IdentityServer_Common.Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IdentityServer_Common.Services
{
    public class Validation : IValidation
    {
        private readonly IValidationProvider _provider;
        private readonly ILogger<Validation>? _logger;

        public Validation(IValidationProvider provider,
            ILogger<Validation>? logger)
        {
            _provider = provider
                ?? throw new ArgumentNullException(nameof(provider));
            _logger = logger;
        }

        public async Task ValidateAsync<TModel>(TModel model, ModelStateDictionary modelStateDictionary)
        {
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException(nameof(modelStateDictionary));
            }

            var validationResult = await _provider
                .GetValidator<TModel>()
                .ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(modelStateDictionary);

                if (_logger != null)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        _logger.LogDebug($"Validation [Login] Error:\t {error}");
                    }
                }
            }
        }
    }
}
