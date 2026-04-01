namespace TicketApi.Modules.Identity.DTOs
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
