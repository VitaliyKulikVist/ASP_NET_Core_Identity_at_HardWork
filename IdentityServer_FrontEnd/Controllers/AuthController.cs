using IdentityServer_DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Services;
using System.Threading.Tasks;
using Serilog;
using Microsoft.Extensions.Hosting;
using IdentityServer_Common.Infrastructure.Interface;
using IdentityServer_FrontEnd.ViewModels;
using IdentityServer_DAL.Entity.ViewModel.Auth;
using System.Linq;

namespace IdentityServer_FrontEnd.Controllers
{
    public class AuthController : Controller
    {
        /// <summary>
        /// Необхідний для реалізації авторизації(входу)
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Необхідний для пошуку, та створення користувачів
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Необхідний для розлогінення
        /// </summary>
        private readonly IIdentityServerInteractionService _interactionService;

        private readonly IValidation _validation;

        private readonly IHostEnvironment _environment;

        public AuthController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IIdentityServerInteractionService identityServerInteractionService,
            IValidation validation,
            IHostEnvironment hostEnvironment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = identityServerInteractionService;

            _validation = validation;

            _environment = hostEnvironment;
        }


        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            if( _environment.IsDevelopment())
            {
                Log.Debug("Login [Get] RedirectURL:{returnURL}", 
                    string.IsNullOrWhiteSpace(returnUrl) ? "Empty" : returnUrl);
            }

            var viewModel = new LoginViewModel 
            { 
                ReturnUrl = returnUrl
            };

            return View("Login", viewModel);
        }

        /// <summary>
        /// Сюди буде переходити керування із фори Логін
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns>Буде повертати туди, звідки прийшов запит</returns>
        [HttpPost]
        public async Task<IActionResult> Login (LoginViewModel loginViewModel)
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("Try Login [Post] user:\tName: {UserName}\tPassword: {Password}\nRedirectURL:\t{ReturnUrl}", loginViewModel.UserName,
                    loginViewModel.Password,
                    string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl) ? "Empty": loginViewModel.ReturnUrl);
            }

            await _validation.ValidateAsync(loginViewModel, ModelState);

            if (!ModelState.IsValid)
            {
                var errorModel = new ErrorViewModel();
                var errors = ModelState.Values.SelectMany(s => s.Errors);
                var userNameTemp = errors.Where(s => s.ErrorMessage.Contains("User Name"));
                errorModel.UserNameErrors = userNameTemp;
                var passwordTemp = errors.Where(s => s.ErrorMessage.Contains(nameof(loginViewModel.Password)));
                errorModel.PasswordErrors = passwordTemp;

                ViewBag.ErrorModel = errorModel;//Не можна використовувати динамік, в подальшому переробити

                return View("Login", loginViewModel);
            }

            if (_environment.IsDevelopment())
            {
                Log.Debug("Validation [Login] Done!");
            }

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user == null)
            {
                if (_environment.IsDevelopment())
                {
                    Log.Debug("User NOT found at:\tname: {UserName}\tpassword: {Password}", loginViewModel.UserName, loginViewModel.Password);
                }

                ModelState.AddModelError(string.Empty, "User not found");

                return View("Login", loginViewModel );
                //return RedirectToAction("Register", "Auth", loginViewModel);
            }

            //HttpContext.User.

            //isPersistent - параметр відноситься до Cookie
            //lockoutOnFailure - параметр відповідає за те, щоб заблокувати акаунт якщо було декілька не вдалих спроб
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);

            if (result.Succeeded && _environment.IsDevelopment())
            {
                Log.Information("USER\t{UserName}\t Found!", loginViewModel.UserName);
            }

            if (result.Succeeded)// && !string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl)) 
            {
                //return Redirect(loginViewModel.ReturnUrl!);

                return RedirectToAction("MainPage", "Pages", loginViewModel);
            }

            ModelState.AddModelError(string.Empty, "Login error");

            return View("Login", loginViewModel);
        }

        /// <summary>
        /// Метод необхідний для повернення форми
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register (string? returnUrl)
        {

            if (_environment.IsDevelopment())
            {
                Log.Debug("Register [Get] RedirectURL:{returnURL}",
                    string.IsNullOrWhiteSpace(returnUrl) ? "Empty" : returnUrl);
            }

            var vievModeel = new RegisterViewModel 
            { 
                ReturnUrl = returnUrl 
            };

            return View("Register", vievModeel);
        }
        /// <summary>
        /// Метод необхідний для дій реєстрації, ПІСЛЯ заповнення форми
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("Try Register [Post] user:\tName: {UserName}\tPassword: {Password}\tConfirmPassword: {ConfirmPassword}\nRedirectURL:\t{ReturnUrl}",
                    registerViewModel.UserName,
                    registerViewModel.Password,
                    registerViewModel.ConfirmPassword,
                    string.IsNullOrWhiteSpace(registerViewModel.ReturnUrl) ? "Empty" : registerViewModel.ReturnUrl);
            }

            await _validation.ValidateAsync(registerViewModel, ModelState);

            if (!ModelState.IsValid)
            {
                var errorModel = new ErrorViewModel();
                var errors = ModelState.Values.SelectMany(s => s.Errors);
                var userNameTemp = errors.Where(s => s.ErrorMessage.Contains("User Name"));
                errorModel.UserNameErrors = userNameTemp;
                var passwordTemp = errors.Where(s => s.ErrorMessage.Contains("Password") && !s.ErrorMessage.Contains("Confirm"));
                errorModel.PasswordErrors = passwordTemp;
                var confirmPasswordTemp = errors.Where(s => s.ErrorMessage.Contains("Confirm Password"));
                errorModel.ConfirmPasswordErrors = confirmPasswordTemp;

                ViewBag.ErrorModel = errorModel;


                return View("Register", registerViewModel);
            }

            if (_environment.IsDevelopment())
            {
                Log.Debug($"Validation [Register] Done!");
            }

            var user = new ApplicationUser
            {
                UserName =  registerViewModel.UserName
            };

            //Створюємо користувача за допомогою "userManager"
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (result.Succeeded) 
            {
                //Робимо вхід цього користувача
                await _signInManager.SignInAsync(user, isPersistent: false);

                return Redirect(registerViewModel.ReturnUrl!);
            }

            ModelState.AddModelError(string.Empty, "Error creating user");

            return View("Register", registerViewModel);
        }

        /// <summary>
        /// Метод необхідний для виходу користувача
        /// </summary>
        /// <param name="logOutId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> LogOut(string logOutId)
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("LogOut ID:\t{logOutId}", logOutId);
            }

            await _signInManager.SignOutAsync();

            var logOutRequest = await _interactionService.GetLogoutContextAsync(logOutId);

            return Redirect(logOutRequest.PostLogoutRedirectUri);
        }
    }
}
