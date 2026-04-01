namespace TicketApi.Modules.Identity.Entities
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public Permission Permissions { get; set; } = null!;
    }
}