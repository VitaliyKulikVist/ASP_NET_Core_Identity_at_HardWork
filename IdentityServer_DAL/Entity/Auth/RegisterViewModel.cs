using System.ComponentModel.DataAnnotations;

namespace IdentityServer_DAL.Entity.Auth
{
    /// <summary>
    /// Клас являє собою сутність для реєстрації користувача
    /// </summary>
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]//Зєднуємо поля, що повинно слугувати показником чи це поле відповідає полю "Password"
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
