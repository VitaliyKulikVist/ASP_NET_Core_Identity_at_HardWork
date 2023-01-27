using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccesRequestTest
    {
        public bool HaveAcces { get; } = true;

        public AccesRequestTest(bool haveAcces = true)
        {
            HaveAcces = haveAcces;
        }
    }

    [Route("TakeAccesAPI")]
    [Authorize]
    public class TakeAccesController : ControllerBase
    {
            [HttpGet]
            public IActionResult GetAcces()
            {
                return new JsonResult(new AccesRequestTest());
            }
    }
}
