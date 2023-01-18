using IdentityServer_DAL.Infrastructure.Interface;

namespace IdentityServer_FrontEnd.ViewModels
{
    public class MainPageViewModel
    {
        public string? UserName { get; set; }

        public MainPageViewModel GetMainPage<TViewModel> (TViewModel viewModel)
            where TViewModel : class , IAuthViewModel
        {
            UserName = viewModel.UserName;

            return this;
        }
    }
}
