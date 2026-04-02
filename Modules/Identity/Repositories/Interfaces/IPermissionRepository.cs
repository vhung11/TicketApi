using TicketApi.Modules.Identity.Entities;

namespace TicketApi.Modules.Identity.Repositories.Interfaces
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        Task<Permission?> GetByCodeAsync(string code);
        Task<IEnumerable<Permission>> GetByResourceAsync(string resource);
    }
}