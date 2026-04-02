using TicketApi.Modules.Identity.Entities;

namespace TicketApi.Modules.Identity.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<Role>> GetRolesAsync(int userId);
        Task<IEnumerable<Permission>> GetPermissionsAsync(int userId);
        Task<UserRole?> GetUserRoleAsync(int userId, int roleId);
        Task<UserPermission?> GetUserPermissionAsync(int userId, int permissionId);
        Task AssignRoleAsync(int userId, int roleId);
        Task RemoveRoleAsync(int userId, int roleId);
        Task AssignPermissionAsync(int userId, int permissionId);
        Task RemovePermissionAsync(int userId, int permissionId);
        Task<bool> HasPermissionAsync(int userId, string permissionCode);
    }
}