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

namespace WUM.Lib.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly ISqlConnectionFactory dapper;
        private readonly HtmlEncoder htmlEncoder;
        private readonly ILogger<UserInfoService> logger;

        public UserInfoService(ISqlConnectionFactory dapper, HtmlEncoder htmlEncoder, ILogger<UserInfoService> logger) 
        {
            this.dapper = dapper;
            this.htmlEncoder = htmlEncoder;
            this.logger = logger;
        }

        public async Task<CommonAPIModel<List<UserInfoVM>>> GetUserInfos(string username, string email)
        {
            try
            {
                string cUser = htmlEncoder.Encode(username.Trim());
                string cEmail = htmlEncoder.Encode(email.Trim());
                var param = new DynamicParameters();
                string qSql = " SELECT * " +
                                " FROM UserInfo " +
                                " WHERE 1=1 ";

                if (!cUser.IsNullOrEmpty())
                {
                    qSql += " AND Username = @Username ";
                    param.Add("Username", cUser);
                }
                if (!cEmail.IsNullOrEmpty())
                {
                    qSql += " AND Email = @Email ";
                    param.Add("Email", cEmail);
                }

                using var conn = dapper.CreateConnection();
                var userInfos = (await conn.QueryAsync<UserInfoVM>(qSql, param)).ToList();

                return new CommonAPIModel<List<UserInfoVM>>
                {
                    Data = userInfos
                };
            }
            catch (Exception ex) 
            {
                logger.LogError($"GetUserInfos: {ex.Message}");
                return new CommonAPIModel<List<UserInfoVM>>
                {
                    Success = false,
                    Msg = "系統忙碌中",
                    Data = new List<UserInfoVM>()
                };
            }
        }
    }
}
