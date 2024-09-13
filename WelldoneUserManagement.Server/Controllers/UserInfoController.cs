using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WUM.Lib.Interfaces;

namespace WelldoneUserManagement.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserInfoService userInfoService;

        public UserInfoController(IUserInfoService userInfoService)
        {
            this.userInfoService = userInfoService;
        }

        [HttpGet]
        public async Task<IActionResult> UserInfo()
        {
            var userInfos = await userInfoService.GetUserInfos();
            return Ok(userInfos);
        }
    }
}
