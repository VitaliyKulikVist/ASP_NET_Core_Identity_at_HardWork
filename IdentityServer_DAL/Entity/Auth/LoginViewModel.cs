using System.ComponentModel.DataAnnotations;

namespace IdentityServer_DAL.Entity.Auth
{
    /// <summary>
    /// Клас являє собою сутність для авторизації користувача
    /// </summary>
    public class LoginViewModel
    {
        [Required]//Обов'язкове поле
        public string UserName { get; set; } = null!;
        [Required]//Обов'язкове поле
        [DataType(DataType.Password)]//Атрибут, щоб при вводі даних пароль не відображався
        public string Password { get; set; } = null!;
        public string ReturnUrl { get; set; } = null!;
    }
}
