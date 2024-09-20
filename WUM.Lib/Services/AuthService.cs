using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using WUM.Lib.Interfaces;
using WUM.Lib.Models.Auth;
using WUM.Lib.Models.Common;
using WUM.Lib.Models.JWT;
using WUM.Lib.Models.LogModel;
using WUM.Lib.Utilities;
using static System.Net.Mime.MediaTypeNames;

namespace WUM.Lib.Services
{
    public class AuthService : IAuthService
    {
        private readonly ISqlConnectionFactory dapper;
        private readonly HtmlEncoder htmlEncoder;
        private readonly ILogger<AuthService> logger;
        private readonly IConfiguration config;
        private readonly JWTBase jwtServices;
        private readonly IHttpClientFactory httpClientFactory;

        public AuthService(ISqlConnectionFactory dapper, HtmlEncoder htmlEncoder, ILogger<AuthService> logger, IConfiguration config,
            JWTBase jwtServices, IHttpClientFactory httpClientFactory)
        {
            this.dapper = dapper;
            this.htmlEncoder = htmlEncoder;
            this.logger = logger;
            this.config = config;
            this.jwtServices = jwtServices;
            this.httpClientFactory=httpClientFactory;
        }

        public async Task<CommAPIModel<LoginRes>> PasswordLogin(PasswordLoginReq req)
        {
            string msg = "";
            if (!ChkLoginReq(req))
            {
                msg = "帳號或密碼空白";
                return LoginErrorResponse(msg);
            }

            string loginApi = config["WelldoneAuth:PasswordLoginAPI"];
            var reqJson = new StringContent(
                JsonSerializer.Serialize(req),
                Encoding.UTF8,
                Application.Json);
            var httpClient = httpClientFactory.CreateClient();
            var httpResMsg = await httpClient.PostAsync(loginApi, reqJson);
            if (!httpResMsg.IsSuccessStatusCode)
            {
                msg = "帳密錯誤";
                logger.LogInformation($"PasswordLogin(): {msg}");
                return LoginErrorResponse(msg);
            }
            string httpJson = await httpResMsg.Content.ReadAsStringAsync();
            var wdAuthRes = JsonSerializer.Deserialize<WelldoneAuthPasswordLoginRes>(httpJson);

            var jwtRes = GetJwt();
            if (!jwtRes.isSuccess)
            {
                msg = "token產生失敗";
                logger.LogInformation($"PasswordLogin(): {msg}");
                return LoginErrorResponse(msg);
            }

            msg = "登入成功";
            // 寫log
            await LoginSuccLog(msg, req.Username, "AuthService PasswordLogin()");
            return new CommAPIModel<LoginRes>
            {
                Msg = msg,
                Data = new LoginRes
                {
                    DisplayName = wdAuthRes.Data.DisplayName,
                    Token = jwtRes.jwt
                }
            };
        }

        public async Task<CommAPIModel<QrcodeLoginPrepareRes>> QrcodeLoginPrepare()
        {
            string msg = "Qrcode產生成功";

            string qrcodeLoginPrepareApi = config["WelldoneAuth:QrcodeLoginPrepareAPI"];
            var reqJson = new StringContent(
                JsonSerializer.Serialize(""),
                Encoding.UTF8,
                Application.Json);
            var httpClient = httpClientFactory.CreateClient();
            var httpResMsg = await httpClient.PostAsync(qrcodeLoginPrepareApi, reqJson);
            if (httpResMsg.IsSuccessStatusCode)
            {
                string httpJson = await httpResMsg.Content.ReadAsStringAsync();
                var wdAuthRes = JsonSerializer.Deserialize<WelldoneAuthQrcodeLoginPrepareRes>(httpJson);
                logger.LogInformation($"QrcodeLoginPrepare(): {msg}");
                return new CommAPIModel<QrcodeLoginPrepareRes>
                {
                    Msg = msg,
                    Data = new QrcodeLoginPrepareRes
                    {
                        QrcodeGuid = wdAuthRes.Data.QrcodeGuid,
                    }
                };
            }

            msg = "Qrcode產生失敗";
            logger.LogInformation($"QrcodeLoginPrepare(): {msg}");
            return new CommAPIModel<QrcodeLoginPrepareRes>
            {
                Success = false,
                Data = new QrcodeLoginPrepareRes()
            };
        }

        public async Task<CommAPIModel<LoginRes>> QrcodeLogin(QrcodeLoginReq req)
        {
            string msg = "";
            string qrcodeLoginApi = config["WelldoneAuth:QrcodeLoginAPI"];

            var reqJson = new StringContent(
                JsonSerializer.Serialize(req),
                Encoding.UTF8,
                Application.Json);

            var httpClient = httpClientFactory.CreateClient();
            var httpResMsg = await httpClient.PostAsync(qrcodeLoginApi, reqJson);
            if (httpResMsg.IsSuccessStatusCode)
            {
                string httpJson = await httpResMsg.Content.ReadAsStringAsync();
                var wdAuthRes = JsonSerializer.Deserialize<WelldoneAuthQrcodeLoginRes>(httpJson);

                var jwtRes = GetJwt();
                if (jwtRes.isSuccess)
                {
                    msg = "登入成功";
                    // 寫log
                    await LoginSuccLog(msg, wdAuthRes.Data.DisplayName, "AuthService QrcodeLogin()");
                    return new CommAPIModel<LoginRes>
                    {
                        Data = new LoginRes
                        {
                            DisplayName = wdAuthRes.Data.DisplayName,
                            Token = jwtRes.jwt
                        }
                    };
                }
            }
            msg = "登入失敗";
            logger.LogInformation($"QrcodeLogin(): {msg}");
            return LoginErrorResponse(msg);
        }

        private RunStatus GetJwt()
        {
            JWTCliam jwtCliam = new()
            {
                iss = config["JWTConfig:Issuer"]
            };
            string key = config["JWTConfig:SignKey"];
            string iss = config["JWTConfig:Issuer"];
            return jwtServices.GetJWT(jwtCliam, key, iss);
        }

        private bool ChkLoginReq(PasswordLoginReq req)
        {
            return !string.IsNullOrEmpty(req.Username) && !string.IsNullOrEmpty(req.Password);
        }

        private CommAPIModel<LoginRes> LoginErrorResponse(string msg)
        {
            return new CommAPIModel<LoginRes>
            {
                Success = false,
                Msg = msg,
                Data = new LoginRes()
            };
        }

        private async Task LoginSuccLog(string msg, string userId, string source)
        {
            // 寫log
            using var conn = dapper.CreateConnection();
            WelldoneLog log = new WelldoneLog
            {
                LogDate = TimeUtil.UtcNow(),
                LogMsg = msg,
                UserId = userId,
                LogSource = source
            };
            await LogUtil.Save(conn, log);
        }
    }
}
