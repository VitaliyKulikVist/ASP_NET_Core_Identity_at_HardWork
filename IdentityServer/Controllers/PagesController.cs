using IdentityServer.ViewModels;
using IdentityServer_DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class PagesController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHostEnvironment _environment;

        public PagesController (
            UserManager<ApplicationUser> userManager,
            IHostEnvironment environment)
        {
            _userManager = userManager;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> MainPage(MainPageViewModel mainPageViewModel)
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("Try MainPage [Get] user:\tName: {UserName}\tPassword: {Password}\nRedirectURL:\t{ReturnUrl}", mainPageViewModel.UserName);
            }

            if (_environment.IsDevelopment())
            {
                Log.Debug("Validation [MainPage] Done!");
            }

            if(mainPageViewModel != null && !string.IsNullOrWhiteSpace(mainPageViewModel.UserName))//Заглушка, потім забрати
            {
                var user = await _userManager.FindByNameAsync(mainPageViewModel.UserName);
                if (user == null)
                {
                    if (_environment.IsDevelopment())
                    {
                        Log.Debug("User NOT found at:\tname: {UserName}", mainPageViewModel.UserName);
                    }

                    ModelState.AddModelError(string.Empty, "User not found");

                    return View("Login", mainPageViewModel);
                }
            }

            //Заглушка потім забрати
            else
            {
                mainPageViewModel = new MainPageViewModel
                {
                    UserName = "TestUserName"
                };
            }

            return View("MainPage", mainPageViewModel);
        }
    }
}
