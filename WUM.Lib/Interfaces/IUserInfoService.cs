using WUM.Lib.Models.Common;
using WUM.Lib.Models.UserInfo;

namespace WUM.Lib.Interfaces
{
    public interface IUserInfoService
    {
        Task<CommAPIModel<List<UserInfoVM>>> GetUserInfos(UserInfoReq req);
        Task<CommAPIModel<UserDetailVM>> GetUserDetail(int id);
        Task<CommAPIModel<string>> CreateUser(UserCreateReq req);
        Task<CommAPIModel<string>> DeleteUser(int id);
        Task<CommAPIModel<string>> EditUser(UserEditReq req);
    }
}
