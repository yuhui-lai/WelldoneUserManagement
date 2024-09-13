using System.Text.Json.Serialization;

namespace WUM.Lib.Models.Auth
{
    public class WelldoneAuthPasswordLoginRes
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
        public WelldoneAuthPasswordLoginResData Data { get; set; }
    }

    public class WelldoneAuthPasswordLoginResData
    {
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
    }
}
