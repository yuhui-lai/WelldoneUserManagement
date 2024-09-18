using System.ComponentModel.DataAnnotations;

namespace WUM.Lib.Models.UserInfo
{
    public class UserCreateReq
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Country { get; set; }
        public bool Status { get; set; }
        public List<ProjectRole>? ProjectRoles { get; set; }

    }

    public class ProjectRole
    {
        public string? ProjectName { get; set; }
        public List<string>? RoleName { get; set; }
    }
}
