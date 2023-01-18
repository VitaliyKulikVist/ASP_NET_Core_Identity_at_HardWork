using IdentityServer_DAL.Infrastructure.Interface;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer_DAL.Entity.ViewModel.Auth
{
    /// <summary>
    /// Клас являє собою сутність для авторизації користувача
    /// </summary>
    public class LoginViewModel : IAuthViewModel
    {
        public string UserName { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public string? ReturnUrl { get; set; }
    }
}
