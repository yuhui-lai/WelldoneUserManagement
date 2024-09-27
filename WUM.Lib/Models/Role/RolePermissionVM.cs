namespace WUM.Lib.Models.Role
{
    public class RolePermissionVM
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public List<RoleItemPermission> Items { get; set; }
        public List<string> MainItems { get; set; }
    }

    public class RoleItemPermission
    {
        public string MainItem { get; set; }
        public string SubItem { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Delete { get; set; }
        public bool Export { get; set; }
    }
}
