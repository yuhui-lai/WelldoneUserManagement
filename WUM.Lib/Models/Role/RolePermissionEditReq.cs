namespace WUM.Lib.Models.Role
{
    public class RolePermissionEditReq
    {
        public string OperatorId { get; set; }
        public int RoleId { get; set; }
        public List<RolePermissionEditItem> RolePermissionItems { get; set; }
    }

    public class RolePermissionEditItem
    {
        public string MainItem { get; set; }
        public string SubItem { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Delete { get; set; }
        public bool Export { get; set; }
    }
}