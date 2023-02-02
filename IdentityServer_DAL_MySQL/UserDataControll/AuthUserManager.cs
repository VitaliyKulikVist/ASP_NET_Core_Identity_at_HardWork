using IdentityServer_DAL.Entity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServer_DAL_MySQL.UserDataControll
{
    public class AuthUserManager
    {
        /// <summary>
        /// Необхідний для реалізації авторизації(входу)
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Необхідний для пошуку, та створення користувачів
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthUserManager(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return user;
        }

        /// <summary>
        /// Метод авторизування користувача за Іменем та паролем
        /// </summary>
        /// <remarks>
        /// isPersistent - параметр відноситься до Cookie
        /// lockoutOnFailure - параметр відповідає за те, щоб заблокувати акаунт якщо було декілька не вдалих спроб
        /// </remarks>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <param name="lockoutOnFailure"></param>
        public async Task<SignInResult> SignInAsync(string userName, string password,bool isPersistent = false, bool lockoutOnFailure = false)
        {
            var result = await _signInManager.PasswordSignInAsync(
                userName,
                password,
                isPersistent: isPersistent,
                lockoutOnFailure: lockoutOnFailure);

            return result;
        }

        public Task SignInAsync(ApplicationUser user, bool isPersistent = false, string authenticationMethod = null!)
        {
            var result = _signInManager.SignInAsync(user, isPersistent:  isPersistent, authenticationMethod: authenticationMethod);

            return result;
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            return result;
        }

        public async Task<IdentityResult> AddClaimsAtUserAsync(ApplicationUser user, IEnumerable<Claim> claims)
        {
            var result = await _userManager.AddClaimsAsync(user, claims);

            return result;
        }

        public async Task SignOutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
