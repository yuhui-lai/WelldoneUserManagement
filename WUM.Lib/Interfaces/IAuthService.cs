using WUM.Lib.Models.Auth;
using WUM.Lib.Models.Common;

namespace WUM.Lib.Interfaces
{
    public interface IAuthService
    {
        Task<CommAPIModel<LoginRes>> PasswordLogin(PasswordLoginReq req);
        Task<CommAPIModel<QrcodeLoginPrepareRes>> QrcodeLoginPrepare();
        Task<CommAPIModel<LoginRes>> QrcodeLogin(QrcodeLoginReq req);
        
    }
}
