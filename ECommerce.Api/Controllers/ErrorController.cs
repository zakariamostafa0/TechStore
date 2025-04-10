using ECommerce.Api.Helper;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("errors/{statusCode}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult HandleError(int statusCode)
        {
            return new ObjectResult(new ResponseAPI(statusCode));
        }
    }
}
