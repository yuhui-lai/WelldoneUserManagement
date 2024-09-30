namespace WUM.Lib.Models.Role
{
    public class RolePermissionVM
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<RolePermissionItem> RolePermissionItems { get; set; }
    }

    public class RolePermissionItem
    {
        public string MainItem { get; set; }
        public string SubItem { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Delete { get; set; }
        public bool Export { get; set; }
    }
}
