using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Linq;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>
    /// Цей контролер використовуватиметься для перевірки вимог авторизації
    /// </summary>
    /// <remarks>
    /// Також для візуалізації ідентичності претензій через API
    /// </remarks>
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [Route("identity/get")]
        public IActionResult Get()
        {
            var res = User.Claims.Select(s => new
            {
                Type_Claim = s.Type,
                Value_Claim =  s.Value
            });

            var ss = res.First(s => s.Value_Claim == "asdas");

            return new JsonResult(res);
        }

        [HttpGet]
        [Authorize]
        [Route("identity/get/{type}")]
        public IActionResult GetCorrentcytype(string? type)
        {
            var sortClaims = User.Claims.Where(c => c.Type.Equals(type));

            foreach (var item in sortClaims)
            {
                Log.Error(item.Type);
            }
            
            if (sortClaims != null && sortClaims.Count() > 0)
            {
                return Ok(sortClaims);// new JsonResult(sortClaims);
            }

            return NotFound($"{type} claim does not exist");
        }

    }
}
