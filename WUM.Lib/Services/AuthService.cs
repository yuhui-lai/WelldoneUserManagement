using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using WUM.Lib.Interfaces;
using WUM.Lib.Models.Auth;
using WUM.Lib.Models.Common;
using WUM.Lib.Models.JWT;
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

        public async Task<CommonAPIModel<LoginRes>> PasswordLogin(PasswordLoginReq req)
        {
            string loginApi = config["WelldoneAuth:PasswordLoginAPI"];

            var reqJson = new StringContent(
                JsonSerializer.Serialize(req),
                Encoding.UTF8,
                Application.Json);

            var httpClient = httpClientFactory.CreateClient();
            var httpResMsg = await httpClient.PostAsync(loginApi, reqJson);

            if (httpResMsg.IsSuccessStatusCode)
            {
                string httpJson = await httpResMsg.Content.ReadAsStringAsync();
                var wdAuthRes = JsonSerializer.Deserialize<WelldoneAuthPasswordLoginRes>(httpJson);

                var jwtRes = GetJwt();
                if (jwtRes.isSuccess)
                {
                    return new CommonAPIModel<LoginRes>
                    {
                        Msg = "登入成功",
                        Data = new LoginRes
                        {
                            DisplayName = wdAuthRes.Data.DisplayName,
                            Token = jwtRes.jwt
                        }
                    };
                }
                return new CommonAPIModel<LoginRes>
                {
                    Success = false,
                    Msg = "token產生失敗",
                    Data = new LoginRes()
                };
            }
            return new CommonAPIModel<LoginRes>
            {
                Success = false,
                Msg = "帳密錯誤",
                Data = new LoginRes()
            };
        }

        public async Task<CommonAPIModel<QrcodeLoginPrepareRes>> QrcodeLoginPrepare()
        {
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

                return new CommonAPIModel<QrcodeLoginPrepareRes>
                {
                    Data = new QrcodeLoginPrepareRes
                    {
                        QrcodeGuid = wdAuthRes.Data.QrcodeGuid,
                    }
                };
            }
            return new CommonAPIModel<QrcodeLoginPrepareRes>
            {
                Success = false,
                Data = new QrcodeLoginPrepareRes()
            };
        }

        public async Task<CommonAPIModel<LoginRes>> QrcodeLogin(QrcodeLoginReq req)
        {
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
                    return new CommonAPIModel<LoginRes>
                    {
                        Data = new LoginRes
                        {
                            DisplayName = wdAuthRes.Data.DisplayName,
                            Token = jwtRes.jwt
                        }
                    };
                }
            }
            return new CommonAPIModel<LoginRes>
            {
                Success = false,
                Data = new LoginRes()
            };
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


    }
}
