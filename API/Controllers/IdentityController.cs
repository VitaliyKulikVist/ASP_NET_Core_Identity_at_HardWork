using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace API.Controllers
{
    /// <summary>
    /// Цей контролер використовуватиметься для перевірки вимог авторизації
    /// </summary>
    /// <remarks>
    /// Також для візуалізації ідентичності претензій через API
    /// </remarks>
    [Route("identity")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
