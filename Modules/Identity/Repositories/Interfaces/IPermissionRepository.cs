using TicketApi.Modules.Identity.Entities;

namespace TicketApi.Modules.Identity.Repositories.Interfaces
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        Task<Permission?> GetByCodeAsync(string code);
        Task<IEnumerable<Permission>> GetByResourceAsync(string resource);

        /// <summary>
        /// Checks if a user has a specific permission code,
        /// either through their roles or through direct assignment.
        /// </summary>
        Task<bool> UserHasPermissionAsync(int userId, string permissionCode);

        Task<IEnumerable<Permission>> GetPermissionsAsync(int userId);
    }
}