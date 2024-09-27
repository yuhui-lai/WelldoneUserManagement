using WUM.Lib.Models.Common;
using WUM.Lib.Models.Role;

namespace WUM.Lib.Interfaces
{
    public interface IRoleService
    {
        Task<CommAPIModel<List<RoleVM>>> GetRolesAsync(RoleListReq req);
        Task<CommAPIModel<List<ProductClassRes>>> GetProductClassesAsync();
        Task<CommAPIModel<RolePermissionVM>> GetRolePermissionAsync(int id);
        Task<CommAPIModel<RoleBasicInfoVM>> GetRoleBasicInfoAsync(int id);
        Task<CommAPIModel<string>> CreateRoleAsync(RoleCreateReq req);
        Task<CommAPIModel<string>> DeleteRoleAsync(RoleDeleteReq req);
        Task<CommAPIModel<string>> EditRolePermissionAsync(RolePermissionEditReq req);
        Task<CommAPIModel<string>> EditRoleBasicInfoAsync(RoleBasicInfoEditReq req);
    }
}
