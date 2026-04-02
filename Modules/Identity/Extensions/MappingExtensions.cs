using TicketApi.Modules.Identity.DTOs;
using TicketApi.Modules.Identity.Entities;

namespace TicketApi.Modules.Identity.Extensions
{
    public static class MappingExtensions
    {
        public static UserDto ToDto(this User u) => new()
        {
            Id = u.Id,
            Username = u.Name,
            Email = u.Email,
            IsActive = u.IsActive,
            Roles = u.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new List<string>()
        };

        public static RoleDto ToDto(this Role r) => new()
        {
            Id = r.Id,
            Name = r.Name,
            IsActive = r.IsActive
        };

        public static PermissionDto ToDto(this Permission p) => new()
        {
            Id = p.Id,
            Code = p.Code,
            Resource = p.Resource,
            IsActive = p.IsActive
        };
    }
}
