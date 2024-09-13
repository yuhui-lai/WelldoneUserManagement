using System.Text.Json.Serialization;

namespace WUM.Lib.Models.Auth
{
    public class WelldoneAuthQrcodeLoginPrepareRes
    {
        /// <summary>
        /// api狀態
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; } = true;
        /// <summary>
        /// 說明內容
        /// </summary>
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = "";
        [JsonPropertyName("data")]
        public WelldoneAuthQrcodeLoginPrepareResData Data { get; set; }
    }

    public class WelldoneAuthQrcodeLoginPrepareResData
    {
        [JsonPropertyName("qrcodeGuid")]
        public string QrcodeGuid { get; set; }
    }
}
