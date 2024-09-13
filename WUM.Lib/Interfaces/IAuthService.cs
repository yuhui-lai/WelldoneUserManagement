using WUM.Lib.Models.Auth;
using WUM.Lib.Models.Common;

namespace WUM.Lib.Interfaces
{
    public interface IAuthService
    {
        Task<CommonAPIModel<LoginRes>> PasswordLogin(PasswordLoginReq req);
        Task<CommonAPIModel<QrcodeLoginPrepareRes>> QrcodeLoginPrepare();
        Task<CommonAPIModel<LoginRes>> QrcodeLogin(QrcodeLoginReq req);
        
    }
}
