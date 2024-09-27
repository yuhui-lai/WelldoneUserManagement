using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using WUM.Lib.Interfaces;
using WUM.Lib.Models.Common;
using WUM.Lib.Models.DB_Context;
using WUM.Lib.Models.UserInfo;
using WUM.Lib.Utilities;

namespace WUM.Lib.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly ISqlConnectionFactory dapper;
        private readonly HtmlEncoder encoder;
        private readonly ILogger<UserInfoService> logger;
        private readonly ManagementContext dbContext;

        public UserInfoService(ISqlConnectionFactory dapper, HtmlEncoder encoder, ILogger<UserInfoService> logger,
            ManagementContext dbContext)
        {
            this.dapper = dapper;
            this.encoder = encoder;
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public async Task<CommAPIModel<List<UserInfoVM>>> GetUserInfos(UserInfoReq req)
        {
            var cUser = req.Username.Purify(encoder);
            var cDname = req.Displayname.Purify(encoder);
            var cCountry = req.Country.Purify(encoder);

            var data = GetFakeUserInfos();
            var res = data.Where(u =>
                (string.IsNullOrEmpty(cUser) || u.Username.Contains(cUser)) &&
                (string.IsNullOrEmpty(cDname) || u.Displayname.Contains(cDname)) &&
                (req.Status == null || u.Status == req.Status) &&
                (string.IsNullOrEmpty(cCountry) || u.Country.Contains(cCountry))
            ).ToList();

            return new CommAPIModel<List<UserInfoVM>>
            {
                Data = res
            };
        }

        public async Task<CommAPIModel<string>> CreateUser(UserCreateReq req)
        {
            string cOptId = req.OperatorId.Purify(encoder);
            string cUser = req.Username.Purify(encoder);
            string cName = req.DisplayName.Purify(encoder);
            string cPass = req.Password.Purify(encoder);
            string cEmail = req.Email.Purify(encoder);

            await LogUtil.SaveLog(req, cOptId, "UserInfoService CreateUser()", LogLevel.Information.ToString(), dbContext);
            return new CommAPIModel<string>
            {
                Msg = "新增成功",
                Data = $"{cUser}, {cName}, {cPass}, {cEmail}"
            };
        }

        public async Task<CommAPIModel<string>> DeleteUser(int id, UserDeleteReq req)
        {
            string cOptId = req.OperatorId.Purify(encoder);
            var delModel = new
            {
                DeleteId = id,
                Req = req
            };
            await LogUtil.SaveLog(delModel, cOptId, "UserInfoService DeleteUser()", LogLevel.Information.ToString(), dbContext);
            return new CommAPIModel<string>
            {
                Msg = "刪除成功",
                Data = id.ToString()
            };
        }

        public async Task<CommAPIModel<UserDetailVM>> GetUserDetail(int id)
        {
            return new CommAPIModel<UserDetailVM>
            {
                Msg = "取得成功",
                Data = GetFakeUserDetail()
            };
        }   

        public async Task<CommAPIModel<string>> EditUser(UserEditReq req)
        {
            string cOptId = req.OperatorId.Purify(encoder);           
            await LogUtil.SaveLog(req, cOptId, "UserInfoService EditUser()", LogLevel.Information.ToString(), dbContext);
            return new CommAPIModel<string>
            {
                Msg = "編輯成功",
                Data = req.Displayname
            };
        }

        private List<UserInfoVM> GetFakeUserInfos()
        {
            UserInfoVM r1 = new()
            {
                Id = 1,
                Username = "marco@welldone.com.tw",
                Displayname = "marco",
                Status = true,
                Country = "TWN"
            };
            UserInfoVM r2 = new()
            {
                Id = 2,
                Username = "jennifer@welldone.com.tw",
                Displayname = "jennifer",
                Status = true,
                Country = "TWN"
            };
            UserInfoVM r3 = new()
            {
                Id = 3,
                Username = "yuhui@welldone.com.tw",
                Displayname = "賴育暉",
                Status = false,
                Country = "USA"
            };
            List<UserInfoVM> infos = new List<UserInfoVM>();
            infos.Add(r1);
            infos.Add(r2);
            infos.Add(r3);
            return infos;
        }

        private UserDetailVM GetFakeUserDetail()
        {
            UserDetailVM vm = new()
            {
                Id = 5,
                Username = "fake777",
                Displayname = "機器人77",
                Email = "fake@fake.com",
                Country = "TWN",
                Status = true,
                Password = "password",
                System = new SystemRole { }
            };
            return vm;
        }

        //private async Task SaveLog<T>(T req, string operatorId, string logSource)
        //{
        //    var options = new JsonSerializerOptions
        //    {
        //        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        //    };

        //    WelldoneLog log = new WelldoneLog
        //    {
        //        LogDate = TimeUtil.UtcNow(),
        //        LogLevel = LogLevel.Information.ToString(),
        //        LogMsg = JsonSerializer.Serialize(req, options),
        //        UserId = operatorId,
        //        LogSource = logSource
        //    };
        //    await dbContext.WelldoneLog.AddAsync(log);
        //    await dbContext.SaveChangesAsync();
        //}
    }
}
