using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUM.Lib.Models.Role
{
    public class RoleCreateReq
    {
        public string OperatorId { get; set; }
        public string ProductCode { get; set; }
        public string RoleName { get; set; }
    }
}
