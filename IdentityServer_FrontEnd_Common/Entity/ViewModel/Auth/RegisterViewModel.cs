using IdentityServer_FrontEnd_Common.Infrastructure.Interface;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer_FrontEnd_Common.Entity.ViewModel.Auth
{
    /// <summary>
    /// Клас являє собою сутність для реєстрації користувача
    /// </summary>
    public class RegisterViewModel : IAuthViewModel
    {
        public string UserName { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        [DataType(DataType.Url)]
        public string? ReturnUrl { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
    }
}
