﻿using Client.ViewModels;
using IdentityModel.Client;
using IdentityServer_Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class PagesController : Controller
    {
        private readonly IHostEnvironment _environment;

        public PagesController ( IHostEnvironment environment)
        {
            _environment = environment;
        }

        //Загрузка самої сторінки
        [HttpGet]
        public IActionResult TakeAcces(bool accesIs = false)
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("Try Take Acces [Get]");
            }

            var viewModel = new TakeAccesViewModels()
            {
                HaveAcces = accesIs,
            };

            return View(IdentityServerFrontEndConstants.NamePageTakeAcces, viewModel);
        }

        [HttpPost]
        public IActionResult TakeAcces(TakeAccesViewModels takeAccesViewModels)
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("Try Take Acces [Post] acces is = {ass}", 
                    takeAccesViewModels.HaveAcces);
            }

            if (!takeAccesViewModels.HaveAcces)
            {
                return RedirectToAction(IdentityServerFrontEndConstants.NamePageLogin, IdentityServerFrontEndConstants.ControllerNameAuth, "https://localhost:6216/Pages/TakeAcces");
            }

            return View(IdentityServerFrontEndConstants.NamePageTakeAcces, takeAccesViewModels);
        }

        //Кнопка 1
        [HttpGet]
        public IActionResult TakeAccesGrantTypesClientCredentials(bool accesIs)
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("Try Take Acces GrantTypes.ClientCredentials [Get]");
            }

            var viewModel = new TakeAccesViewModels()
            {
                HaveAcces = accesIs,
            };

            return RedirectToAction(actionName: "TakeAccesGrantTypesClientCredentials", controllerName: "Pages", viewModel);
            //return View(IdentityServerFrontEndConstants.NamePageTakeAcces, viewModel);
        }

        [HttpPost]
        public IActionResult TakeAccesGrantTypesClientCredentials(TakeAccesViewModels takeAccesViewModels)
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("Try Take Acces GrantTypes.ClientCredentials [Post] acces is = {ass}",
                    takeAccesViewModels.HaveAcces);
            }

            if (!takeAccesViewModels.HaveAcces)
            {
                return RedirectToAction(IdentityServerFrontEndConstants.NamePageLogin, IdentityServerFrontEndConstants.ControllerNameAuth, "https://localhost:6216/Pages/TakeAcces");
            }

            _ = SendRequestAtIdentityServerAtTryGetAccesFromApi1Async("client", "secret");

            return View(IdentityServerFrontEndConstants.NamePageTakeAcces, takeAccesViewModels);
        }

        private async Task SendRequestAtIdentityServerAtTryGetAccesFromApi1Async(string clientID, string clientSecret)
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                Log.Error(disco.Error);

                return;
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = clientID,
                ClientSecret = clientSecret,
            });

            if (tokenResponse.IsError)
            {
                Log.Error("Error= " + tokenResponse.Error);

                return;
            }

            var parserJson = JsonConvert.DeserializeObject(tokenResponse.Json.ToString());
            if (parserJson != null)
            {
                Log.Information(parserJson.ToString());
            }
   
            // call api
            await Switches1(tokenResponse);
        }

        private Task Switches1(IdentityModel.Client.TokenResponse tokenResponse)
        {
            string url = "https://localhost:7249/identity";

            return GetResponseFromURLAsync(url, tokenResponse);
        }

        private async Task GetResponseFromURLAsync(string url, IdentityModel.Client.TokenResponse tokenResponse)
        {
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Log.Warning("Status code=\t" + response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Log.Information(JArray.Parse(content).ToString());

            }
        }
    }
}
