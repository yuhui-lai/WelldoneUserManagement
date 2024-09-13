using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUM.Lib.Models.Common;
using WUM.Lib.Models.UserInfo;

namespace WUM.Lib.Interfaces
{
    public interface IUserInfoService
    {
        Task<CommonAPIModel<List<UserInfoVM>>> GetUserInfos(string username = "", string email = "");
    }
}
