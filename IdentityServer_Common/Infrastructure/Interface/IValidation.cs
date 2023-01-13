using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace IdentityServer_Common.Infrastructure.Interface
{
    public interface IValidation
    {
        Task ValidateAsync<TModel>(TModel model, ModelStateDictionary modelStateDictionary);
    }
}
