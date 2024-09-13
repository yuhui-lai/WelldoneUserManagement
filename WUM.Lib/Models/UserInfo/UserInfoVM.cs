using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUM.Lib.Models.UserInfo
{
    public class UserInfoVM
    {
        public int Id { get; set; }
        public string Displayname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public string Roles { get; set; }
    }
}
