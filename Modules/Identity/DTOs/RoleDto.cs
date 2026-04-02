namespace TicketApi.Modules.Identity.DTOs
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class CreateRoleDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateRoleDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateRoleStatusDto
    {
        public bool IsActive { get; set; }
    }
}
