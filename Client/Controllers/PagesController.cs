using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Serilog;

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
        public IActionResult TakeAcces()
        {
            if (_environment.IsDevelopment())
            {
                Log.Debug("Try TakeAcces [Get] at Client)");
            }

            return View("TakeAcces");
        }
    }
}
