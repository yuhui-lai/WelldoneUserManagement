using System.ComponentModel.DataAnnotations;

namespace WUM.Lib.Models.UserInfo
{
    public class UserDeleteReq
    {
        [Required]
        public string OperatorId { get; set; }
    }
}
