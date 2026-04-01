using TicketApi.Modules.Identity.Entities;

namespace TicketApi.Modules.Identity.Repositories.Interfaces
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        Permission? GetByCode(string code);
        IEnumerable<Permission> GetByResource(string resource);

        /// <summary>
        /// Checks if a user has a specific permission code,
        /// either through their roles or through direct assignment.
        /// </summary>
        bool UserHasPermission(int userId, string permissionCode);
    }
}