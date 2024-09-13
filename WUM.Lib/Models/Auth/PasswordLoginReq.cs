using System.ComponentModel.DataAnnotations;

namespace WUM.Lib.Models.Auth
{
    public class PasswordLoginReq
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Required]
        public string Username { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
