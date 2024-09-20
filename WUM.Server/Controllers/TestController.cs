using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WUM.Lib.Models.Common;

namespace WUM.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration config;

        public TestController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpGet]
        public IActionResult Env()
        {
            string env = config["Env"];

            var res = new CommAPIModel<string>
            {
                Data = env
            };
            return Ok(res);
        }
    }
}
