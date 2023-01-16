using System.ComponentModel.DataAnnotations;

namespace IdentityServer_DAL.Entity.ViewModel.Auth
{
    /// <summary>
    /// Клас являє собою сутність для реєстрації користувача
    /// </summary>
    public class RegisterViewModel
    {
        public string UserName { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        public string? ReturnUrl { get; set; }
    }
}
