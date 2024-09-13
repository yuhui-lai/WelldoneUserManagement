using Microsoft.AspNetCore.Mvc;
using WUM.Lib.Interfaces;
using WUM.Lib.Models.Auth;
using WUM.Lib.Services;

namespace WelldoneUserManagement.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JWTBase jwtServices;
        private readonly IConfiguration config;
        private readonly IAuthService authService;

        public AuthController(IConfiguration config, JWTBase jwtServices, IAuthService authService)
        {
            this.jwtServices = jwtServices;
            this.config = config;
            this.authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> PasswordLogin(PasswordLoginReq req)
        {
            var res = await authService.PasswordLogin(req);
            if (res.Success)
            {
                return Ok(res);
            }
            return NotFound(res);
        }

        [HttpPost]
        public async Task<IActionResult> QrcodeLoginPrepare()
        {
            var res = await authService.QrcodeLoginPrepare();
            if (res.Success)
            {
                return Ok(res);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, res);
        }

        [HttpPost]
        public async Task<IActionResult> QrcodeLogin(QrcodeLoginReq req)
        {
            var res = await authService.QrcodeLogin(req);
            if (res.Success)
            {
                return Ok(res);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, res);
        }
    }
}
