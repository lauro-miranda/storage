using Microsoft.AspNetCore.Mvc;

namespace LM.Storage.Api.Controllers
{
    [ApiController, Route("")]
    public class MeController : ControllerBase
    {
        public IActionResult Get() => Ok(new
        {
            name = "lm-storage",
            version = "1.0"
        });
    }
}