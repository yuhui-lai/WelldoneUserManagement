using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WUM.Lib.Models.UserInfo
{
    public class UserEditReq
    {
        [Required]
        public string OperatorId { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Displayname { get; set; }
        [Required]
        public string Email { get; set; }
        public string Country { get; set; }
        public bool Status { get; set; }
        [Required]
        public string Password { get; set; }
        public EditSystemRole? System { get; set; }
    }
    public class EditSystemRole
    {
        public string? SystemCode { get; set; }
        public RoleModel? Role { get; set; }
    }

    public class EditRoleModel
    {
        public string? RoleCode { get; set; }
        public bool? RoleActive { get; set; }
    }
}
