using TicketApi.Modules.Identity.Entities;

namespace TicketApi.Modules.Identity.Repositories.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<List<Permission>> GetPermissionsAsync(int roleId);
        Task AssignPermissionAsync(int roleId, int permissionId);
        Task RemovePermissionAsync(int roleId, int permissionId);
    }
}