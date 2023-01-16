using System.ComponentModel.DataAnnotations;

namespace IdentityServer_DAL.Entity.ViewModel.Auth
{
    /// <summary>
    /// Клас являє собою сутність для авторизації користувача
    /// </summary>
    public class LoginViewModel
    {
        public string UserName { get; set; } = null!;

        [DataType(DataType.Password)]//Атрибут, щоб при вводі даних пароль не відображався
        public string Password { get; set; } = null!;
        public string? ReturnUrl { get; set; }
    }
}
