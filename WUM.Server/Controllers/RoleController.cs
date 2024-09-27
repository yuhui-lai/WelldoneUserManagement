using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WUM.Lib.Interfaces;
using WUM.Lib.Models.Role;

namespace WUM.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService roleService;

        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        /// <summary>
        /// 取得角色列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] RoleListReq req)
        {
            var res = await roleService.GetRolesAsync(req);
            if (!res.Success)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        /// <summary>
        /// 取得產品項目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ProductClasses()
        {
            var res = await roleService.GetProductClassesAsync();
            if (!res.Success)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        /// <summary>
        /// 取得角色權限
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Permission(int id)
        {
            var res = await roleService.GetRolePermissionAsync(id);
            if (!res.Success)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        /// <summary>
        /// 取得角色基本資訊
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> BasicInfo(int id)
        {
            var res = await roleService.GetRoleBasicInfoAsync(id);
            if (!res.Success)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]RoleCreateReq req)
        {
            var res = await roleService.CreateRoleAsync(req);
            if (!res.Success)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] RoleDeleteReq req)
        {
            var res = await roleService.DeleteRoleAsync(req);
            if (!res.Success)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        /// <summary>
        /// 編輯角色權限
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> EditPermission([FromBody] RolePermissionEditReq req)
        {
            var res = await roleService.EditRolePermissionAsync(req);
            if (!res.Success)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        /// <summary>
        /// 編輯角色基本資訊
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> EditBasicInfo([FromBody] RoleBasicInfoEditReq req)
        {
            var res = await roleService.EditRoleBasicInfoAsync(req);
            if (!res.Success)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
        
    }
}
