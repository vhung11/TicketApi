using TicketApi.Modules.Identity.Entities;

namespace TicketApi.Modules.Identity.Repositories.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Role? GetByName(string name);

        /// <summary>
        /// Checks if a user has been assigned the specified role by name.
        /// </summary>
        bool IsUserInRole(int userId, string roleName);
    }
}