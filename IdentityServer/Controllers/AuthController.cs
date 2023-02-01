﻿using IdentityServer_DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Services;
using System.Threading.Tasks;
using Serilog;
using Microsoft.Extensions.Hosting;
using IdentityServer_Common.Infrastructure.Interface;
using IdentityServer_DAL.Entity.ViewModel.Auth;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using System;
using IdentityServer_Common.Constants;
using IdentityServer.ViewModels;

namespace IdentityServer.Controllers
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
                Log.Debug("{Login} [Get] RedirectURL:{returnURL}", FrontEndConstants.NamePageLogin,
                    string.IsNullOrWhiteSpace(returnUrl) ? "Empty" : returnUrl);
            }

            var viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(FrontEndConstants.NamePageLogin, viewModel);
        }

        /// <summary>
        /// Сюди буде переходити керування із фори Логін
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns>Буде повертати туди, звідки прийшов запит</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("Try {Login} [Post] user:\tName: {UserName}\tPassword: {Password}\nRedirectURL:\t{ReturnUrl}",
                    FrontEndConstants.NamePageLogin,
                    loginViewModel.UserName,
                    loginViewModel.Password,
                    string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl) ? "Empty": loginViewModel.ReturnUrl);
            }

            await _validation.ValidateAsync(loginViewModel, ModelState);

            if (!ModelState.IsValid)
            {
                SaveValidateInformationAtDynamic();

                return View(FrontEndConstants.NamePageLogin, loginViewModel);
            }

            if (_environment.IsDevelopment())
            {
                Log.Debug("Validation [{Login}] Done!", FrontEndConstants.NamePageLogin);
            }

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user == null)
            {
                if (_environment.IsDevelopment())
                {
                    Log.Debug("User NOT found at:\tname: {UserName}\tpassword: {Password}",
                        loginViewModel.UserName,
                        loginViewModel.Password);
                }

                ModelState.AddModelError(string.Empty, "User not found");

                return View(FrontEndConstants.NamePageLogin, loginViewModel);
            }

            /* /* Cookie дебаг інпут
            if (_environment.IsDevelopment())
            {
                Log.Information("Content Length before {value}\t Cookies Count = {Cookies}", HttpContext.Request.ContentLength, HttpContext.Request.Cookies.Count);

                if(HttpContext.Request.Cookies.Count > 0)
                {
                    foreach (var cookie in HttpContext.Request.Cookies)
                    {
                        if (cookie.Key == "idsrv.session" || cookie.Key == "Identity.Test_Cookie_Name")
                        {
                            Log.Information("Cookie before= {Key}\n {Value}", cookie.Key, cookie.Value);
                        }
                    }
                }
            }
            */

            //isPersistent - параметр відноситься до Cookie
            //lockoutOnFailure - параметр відповідає за те, щоб заблокувати акаунт якщо було декілька не вдалих спроб
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, isPersistent: true, false);

            /* Cookie дебаг оутпут
            if (_environment.IsDevelopment())
            {
                Log.Information("Content Length after {value}\t Cookies Count = {Cookies}", HttpContext.Request.ContentLength, HttpContext.Request.Cookies.Count);

                if (HttpContext.Request.Cookies.Count > 0)
                {
                    foreach (var cookie in HttpContext.Request.Cookies)
                    {
                        if(cookie.Key == "idsrv.session" || cookie.Key == "Identity.Test_Cookie_Name")
                        {
                            Log.Information("Cookie after= {Key}\n {Value}", cookie.Key, cookie.Value);
                        }
                    }
                }
            }
            */

            if (result.Succeeded && _environment.IsDevelopment())
            {
                Log.Information("USER\t{UserName}\t Found!", loginViewModel.UserName);
            }

            if (result.Succeeded && !string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl)) 
            {
                return Redirect(loginViewModel.ReturnUrl!);
            }

            if (result.Succeeded)
            {
                return RedirectToAction(FrontEndConstants.NamePageMainPage, FrontEndConstants.ControllerNamePages, new MainPageViewModel().GetMainPage(loginViewModel));
            }

            ModelState.AddModelError(string.Empty, $"{FrontEndConstants.NamePageLogin} error");

            return View(FrontEndConstants.NamePageLogin, loginViewModel);
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
                Log.Debug("{Register} [Get] RedirectURL:{returnURL}",
                    FrontEndConstants.NamePageRegister,
                    string.IsNullOrWhiteSpace(returnUrl) ? "Empty" : returnUrl);
            }

            var vievModeel = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(FrontEndConstants.NamePageRegister, vievModeel);
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
                Log.Debug("Try {Register} [Post] user:\tName: {UserName}\tPassword: {Password}\tConfirmPassword: {ConfirmPassword}\nRedirectURL:\t{ReturnUrl}",
                    FrontEndConstants.NamePageRegister,
                    registerViewModel.UserName,
                    registerViewModel.Password,
                    registerViewModel.ConfirmPassword,
                    string.IsNullOrWhiteSpace(registerViewModel.ReturnUrl) ? "Empty" : registerViewModel.ReturnUrl);
            }

            await _validation.ValidateAsync(registerViewModel, ModelState);

            if (!ModelState.IsValid)
            {
                SaveValidateInformationAtDynamic();

                return View(FrontEndConstants.NamePageRegister, registerViewModel);
            }

            if (_environment.IsDevelopment())
            {
                Log.Debug("Validation [{Register}] Done!", FrontEndConstants.NamePageRegister);
            }

            var unit = _userManager.FindByNameAsync(registerViewModel.UserName).Result;
            if (unit == null)
            {
                unit = new ApplicationUser
                {
                    UserName = registerViewModel.UserName,
                    Email = $"{registerViewModel.UserName}Smith@email.com",
                    Description = $"Опис користувача {registerViewModel.UserName}",
                    EmailConfirmed = true,
                };

                var result = _userManager.CreateAsync(unit, registerViewModel.Password).Result;

                if (result.Succeeded)
                {
                    //Робимо вхід цього користувача
                    await _signInManager.SignInAsync(unit, isPersistent: true);

                    return RedirectToAction(
                        FrontEndConstants.NamePageMainPage,                 FrontEndConstants.ControllerNamePages,
                        new MainPageViewModel().GetMainPage(registerViewModel));

                    //return Redirect(registerViewModel.ReturnUrl!);
                }

                else
                if (!result.Succeeded)
                {
                    if (_environment.IsDevelopment())
                    {
                        foreach (var item in result.Errors)
                        {
                            Log.Debug("Can`t create new user, because:\t {error}", item.Description);
                        }
                    }

                    if (result.Errors.Count() > 0)
                    {
                        SaveValidateInformationAtDynamic(result);
                    }

                    return View(FrontEndConstants.NamePageRegister, registerViewModel);
                }

                result = _userManager.AddClaimsAsync(unit, new Claim[]{
                            new Claim(JwtClaimTypes.Name, $"{registerViewModel.UserName} Smith"),
                            new Claim(JwtClaimTypes.GivenName, registerViewModel.UserName),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, $"http://{registerViewModel.UserName}.com"),
                        }).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }

            ModelState.AddModelError(string.Empty, "Error creating user");

            return View(FrontEndConstants.NamePageRegister, registerViewModel);
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

            if (string.IsNullOrWhiteSpace(logOutRequest.PostLogoutRedirectUri))
            {
                return RedirectToAction(
                    FrontEndConstants.NamePageLogin,
                    FrontEndConstants.ControllerNameAuth);
            }

            return Redirect(logOutRequest.PostLogoutRedirectUri);
        }

        private void SaveValidateInformationAtDynamic(IdentityResult result = null!)
        {
            ErrorViewModel errorModel = new ErrorViewModel();

            var errors = ModelState.Values.SelectMany(s => s.Errors);
            var userNameTemp = errors.Where(s => s.ErrorMessage.Contains("User Name"));
            if (userNameTemp != null && userNameTemp.Count() > 0)
            {
                errorModel.UserNameErrors = userNameTemp;
            }

            var passwordTemp = errors.Where(s => s.ErrorMessage.Contains("Password") && !s.ErrorMessage.Contains("Confirm"));
            if (passwordTemp != null) 
            {
                errorModel.PasswordErrors = passwordTemp;
            }

            var confirmPasswordTemp = errors.Where(s => s.ErrorMessage.Contains("Confirm Password"));
            if (confirmPasswordTemp != null) 
            {
                errorModel.ConfirmPasswordErrors = confirmPasswordTemp;
            }

            if (result != null)
            {
                errorModel.IdentityError = result.Errors;
            }

            ViewBag.ErrorModel = errorModel;
        }
    }
}