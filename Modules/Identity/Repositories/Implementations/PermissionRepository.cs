using Microsoft.EntityFrameworkCore;
using TicketApi.Infrastructure.Context;
using TicketApi.Modules.Identity.Entities;
using TicketApi.Modules.Identity.Repositories.Interfaces;

namespace TicketApi.Modules.Identity.Repositories.Implementations
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        protected override DbSet<Permission> DbSet => _context.Permissions;

        public async Task<Permission?> GetByCodeAsync(string code) => await DbSet.FirstOrDefaultAsync(p => p.Code == code);

        public async Task<IEnumerable<Permission>> GetByResourceAsync(string resource) =>
            await DbSet.Where(p => p.Resource == resource).ToListAsync();
    }
}