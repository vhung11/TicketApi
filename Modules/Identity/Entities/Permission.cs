namespace TicketApi.Modules.Identity.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}