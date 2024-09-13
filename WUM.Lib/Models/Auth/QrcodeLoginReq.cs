using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUM.Lib.Models.Auth
{
    public class QrcodeLoginReq
    {
        public string Guid { get; set; }
        public string TempToken { get; set; }
    }
}
