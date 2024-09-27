using System.Text.Json;
using System.Text.Json.Serialization;

namespace WUM.Lib.Models.Role
{
    public class ProductClassRes
    {    
        [JsonPropertyName("title")]
        public string ProductName { get; set; }
        [JsonPropertyName("value")]
        public string ProductCode { get; set; }
    }
}
