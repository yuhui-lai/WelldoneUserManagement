using System.Text.Json.Serialization;

namespace WUM.Lib.Models.Auth
{
    public class WelldoneAuthQrcodeLoginRes
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
        public WelldoneAuthQrcodeLoginResData Data { get; set; }
    }

    public class WelldoneAuthQrcodeLoginResData
    {
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
    }
}
