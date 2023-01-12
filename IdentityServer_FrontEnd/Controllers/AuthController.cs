using IdentityServer_DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Services;
using System.Threading.Tasks;
using IdentityServer_DAL.Entity.Auth;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.AspNetCore;

namespace IdentityServer_FrontEnd.Controllers
{
    public class AuthController : Controller
    {
        /// <summary>
        /// Необхідний для реалізації авторизації(входу)
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager = null!;

        /// <summary>
        /// Необхідний для пошуку, та створення користувачів
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager = null!;

        /// <summary>
        /// Необхідний для розлогінення
        /// </summary>
        private readonly IIdentityServerInteractionService _interactionService = null!;

        private readonly IValidator<LoginViewModel> validatorLoginViewModel = null!;
        private readonly IValidator<RegisterViewModel> validatorRegisterViewModel = null!;

        public AuthController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IIdentityServerInteractionService identityServerInteractionService,
            IValidator<LoginViewModel> validatorLoginView,
            IValidator<RegisterViewModel> validatorRegisterView)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = identityServerInteractionService;

            validatorLoginViewModel = validatorLoginView;
            validatorRegisterViewModel = validatorRegisterView;
        }


        [HttpGet]
        public IActionResult Login(string returnURL)
        {
            var viewModel = new LoginViewModel 
            { 
                ReturnUrl = returnURL 
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
            ValidationResult validationResult = await validatorLoginViewModel.ValidateAsync(loginViewModel);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);

                return View("Login", loginViewModel);
            }

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found");

                return View("Login", loginViewModel);
            }

            //HttpContext.User.

            //isPersistent - параметр відноситься до Cookie
            //lockoutOnFailure - параметр відповідає за те, щоб заблокувати акаунт якщо було декілька не вдалих спроб
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);
            if (result.Succeeded && !string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl)) 
            {
                return Redirect(loginViewModel.ReturnUrl!);
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
        public IActionResult Register (string returnUrl)
        {
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
            ValidationResult validationResult = await validatorRegisterViewModel.ValidateAsync(registerViewModel);
            if (!validationResult.IsValid)
            {
                return View("Register", registerViewModel);
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
            await _signInManager.SignOutAsync();

            var logOutRequest = await _interactionService.GetLogoutContextAsync(logOutId);

            return Redirect(logOutRequest.PostLogoutRedirectUri);
        }
    }
}
