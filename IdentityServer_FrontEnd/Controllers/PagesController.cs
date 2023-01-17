using IdentityServer_Common.Infrastructure.Interface;
using IdentityServer_DAL.Entity;
using IdentityServer_DAL.Entity.ViewModel;
using IdentityServer_DAL.Entity.ViewModel.Auth;
using IdentityServer_FrontEnd.ViewModels;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Tasks;

namespace IdentityServer_FrontEnd.Controllers
{
    public class PagesController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IIdentityServerInteractionService _interactionService;

        private readonly IValidation _validation;

        private readonly IHostEnvironment _environment;

        public PagesController (SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IIdentityServerInteractionService interactionService, IValidation validation, IHostEnvironment environment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
            _validation = validation;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> MainPage(LoginViewModel loginViewModel)
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("Try MainPage [Post] user:\tName: {UserName}\tPassword: {Password}\nRedirectURL:\t{ReturnUrl}", loginViewModel.UserName);
            }

            if (_environment.IsDevelopment())
            {
                Log.Debug("Validation [MainPage] Done!");
            }

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user == null)
            {
                if (_environment.IsDevelopment())
                {
                    Log.Debug("User NOT found at:\tname: {UserName}", loginViewModel.UserName);
                }

                ModelState.AddModelError(string.Empty, "User not found");

                return View("Login", loginViewModel);
            }

            ////

            return View("MainPage", loginViewModel);
        }
    }
}
