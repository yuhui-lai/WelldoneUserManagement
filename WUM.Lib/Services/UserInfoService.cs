using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WUM.Lib.Interfaces;
using WUM.Lib.Models.Common;
using WUM.Lib.Models.UserInfo;
using WUM.Lib.Utilities;

namespace WUM.Lib.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly ISqlConnectionFactory dapper;
        private readonly HtmlEncoder encoder;
        private readonly ILogger<UserInfoService> logger;

        public UserInfoService(ISqlConnectionFactory dapper, HtmlEncoder encoder, ILogger<UserInfoService> logger) 
        {
            this.dapper = dapper;
            this.encoder = encoder;
            this.logger = logger;
        }

        public async Task<CommonAPIModel<List<UserInfoRowVM>>> GetUserInfos()
        {
            return new CommonAPIModel<List<UserInfoRowVM>>
            {
                Data = GetFakeUserInfos()
            };
        }

        public async Task<CommonAPIModel<string>> CreateUser(UserCreateReq req)
        {
            string cUser = req.Username.Purify(encoder);
            string cName = req.DisplayName.Purify(encoder);
            string cPass = req.Password.Purify(encoder);
            string cEmail = req.Email.Purify(encoder);

            return new CommonAPIModel<string>
            {
                Msg = "新增成功",
                Data = $"{cUser}, {cName}, {cPass}, {cEmail}"
            };
        }

        private List<UserInfoRowVM> GetFakeUserInfos()
        {
            UserInfoRowVM r1 = new()
            {
                Id = 1,
                Username = "marco@welldone.com.tw",
                Displayname = "marco",
                Status = true,
            };
            UserInfoRowVM r2 = new()
            {
                Id = 2,
                Username = "jennifer@welldone.com.tw",
                Displayname = "jennifer",
                Status = true,
            };
            UserInfoRowVM r3 = new()
            {
                Id = 3,
                Username = "yuhui@welldone.com.tw",
                Displayname = "賴育暉",
                Status = false,
            };
            List<UserInfoRowVM> infos = new List<UserInfoRowVM>();
            infos.Add(r1);
            infos.Add(r2);
            infos.Add(r3);
            return infos;
        }
    }
}
