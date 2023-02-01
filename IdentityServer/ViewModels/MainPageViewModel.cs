using IdentityServer_DAL.Infrastructure.Interface;

namespace IdentityServer.ViewModels
{
    public class MainPageViewModel
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public MainPageViewModel GetMainPage<TViewModel> (TViewModel viewModel)
            where TViewModel : class , IAuthViewModel
        {
            UserName = viewModel.UserName;
            Password = viewModel.Password;
            Email = viewModel.Email;

            return this;
        }
    }
}
