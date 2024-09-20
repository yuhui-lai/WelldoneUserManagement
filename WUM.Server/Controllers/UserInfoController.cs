using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WUM.Lib.Interfaces;
using WUM.Lib.Models.Common;
using WUM.Lib.Models.UserInfo;

namespace WUM.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> UserInfo([FromQuery] UserInfoReq req)
        {
            var userInfos = await userInfoService.GetUserInfos(req);
            if (userInfos.Success)
                return Ok(userInfos);
            return BadRequest(userInfos);
        }

        /// <summary>
        /// 取得帳號詳細資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetail(int id)
        {
            var userInfos = await userInfoService.GetUserDetail(id);
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
        public async Task<IActionResult> CreateUser([FromBody] UserCreateReq req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new CommAPIModel<string>
                {
                    Success = false,
                    Msg = "參數未輸入正確，請確認",
                    Data = ""
                });
            }
            var res = await userInfoService.CreateUser(req);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }

        /// <summary>
        /// 刪除帳號
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var res = await userInfoService.DeleteUser(id);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }

        /// <summary>
        /// 編輯帳號
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> EditUser([FromBody] UserEditReq req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new CommAPIModel<string>
                {
                    Success = false,
                    Msg = "參數未輸入正確，請確認",
                    Data = ""
                });
            }
            var res = await userInfoService.EditUser(req);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
    }
}
