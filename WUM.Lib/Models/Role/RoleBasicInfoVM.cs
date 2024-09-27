using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WUM.Lib.Models.Role
{
    public class RoleBasicInfoVM
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string ProductCode { get; set; }
        public List<RoleBasicProduct> Products { get; set; }
    }

    public class RoleBasicProduct
    {
        [JsonPropertyName("title")]
        public string ProductName { get; set; }
        [JsonPropertyName("value")]
        public string ProductCode { get; set; }
    }
}
