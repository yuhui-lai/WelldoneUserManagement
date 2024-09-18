using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WUM.Lib.Interfaces;
using WUM.Lib.Models.Common;
using WUM.Lib.Models.UserInfo;

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

        /// <summary>
        /// 取得帳號列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UserInfo()
        {
            var userInfos = await userInfoService.GetUserInfos();
            if (userInfos.Success)
                return Ok(userInfos);
            return BadRequest(userInfos);
        }

        /// <summary>
        /// 新增帳號
        /// </summary>
        /// <param name="req">帳號資訊</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]UserCreateReq req)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(new CommonAPIModel<string>
            //    {
            //        Success = false,
            //        Msg = "必填參數未輸入"
            //    });
            var res = await userInfoService.CreateUser(req);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
    }
}
