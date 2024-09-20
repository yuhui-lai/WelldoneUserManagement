namespace WUM.Lib.Models.UserInfo
{
    public class UserDetailVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Displayname { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public bool Status { get; set; }
        public string Password { get; set; }
        public SystemRole System { get; set; }
    }

    public class SystemRole
    {
        public string SystemName { get; set; }
        public string SystemCode { get; set; }
        public RoleModel Role { get; set; }
    }

    public class RoleModel
    {
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public bool RoleActive { get; set; }
    }
}
