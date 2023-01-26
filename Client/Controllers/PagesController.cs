using Client.ViewModels;
using IdentityServer_Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Serilog;
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

        [HttpGet]
        public IActionResult TakeAcces(bool accesIs = false)
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("Try TakeAcces [Get])");
            }

            var viewModel = new TakeAccesViewModels()
            {
                HaveAcces = accesIs,
            };

            return View("TakeAcces", viewModel);
        }

        [HttpPost]
        public IActionResult TakeAcces(TakeAccesViewModels takeAccesViewModels)
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("Try TakeAcces [Post] acces is = {ass}", 
                    takeAccesViewModels.HaveAcces);
            }

            if (!takeAccesViewModels.HaveAcces)
            {
                return RedirectToAction(FrontEndConstants.NamePageLogin, FrontEndConstants.ControllerNameAuth, "https://localhost:6216/Pages/TakeAcces");
            }

            return View("TakeAcces", takeAccesViewModels);
        }
    }
}
