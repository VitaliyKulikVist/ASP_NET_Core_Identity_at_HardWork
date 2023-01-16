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
        public IActionResult MainPage(string? returnUrl)
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("MainPage [Get] RedirectURL:{returnURL}",
                    string.IsNullOrWhiteSpace(returnUrl) ? "Empty" : returnUrl);
            }

            var viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View("MainPage", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> MainPage(LoginViewModel дoginViewModel)
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("Try MainPage [Post] user:\tName: {UserName}\tPassword: {Password}\nRedirectURL:\t{ReturnUrl}", дoginViewModel.UserName);
            }

            await _validation.ValidateAsync(дoginViewModel, ModelState);

            if (!ModelState.IsValid)
            {
                var errorModel = new ErrorViewModel();
                //errorModel.UserNameErrors = new List<string>();
                ViewBag.ErrorModel = errorModel;
                return View("MainPage", дoginViewModel);
            }

            if (_environment.IsDevelopment())
            {
                Log.Debug("Validation [MainPage] Done!");
            }

            var user = await _userManager.FindByNameAsync(дoginViewModel.UserName);
            if (user == null)
            {
                if (_environment.IsDevelopment())
                {
                    Log.Debug("User NOT found at:\tname: {UserName}", дoginViewModel.UserName);
                }

                ModelState.AddModelError(string.Empty, "User not found");

                return View("Login", дoginViewModel);
            }

            ////

            return View("MainPage", дoginViewModel);
        }
    }
}
