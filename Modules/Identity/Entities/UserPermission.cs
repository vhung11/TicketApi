namespace TicketApi.Modules.Identity.Entities
{
    public class UserPermission
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
    }
}