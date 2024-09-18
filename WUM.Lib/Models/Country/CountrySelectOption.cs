using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WUM.Lib.Models.Country
{
    public class CountrySelectOption
    {
        [JsonPropertyName("label")]
        public string Name { get; set; }
        [JsonPropertyName("value")]
        public string Code { get; set; }
        public string EngName { get; set; }
    }
}
