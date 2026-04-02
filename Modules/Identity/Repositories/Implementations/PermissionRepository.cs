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

        public async Task<IEnumerable<Permission>> GetByResourceAsync(string resource) => await DbSet.Where(p => p.Resource == resource).ToListAsync();

        public async Task<bool> UserHasPermissionAsync(int userId, string permissionCode)
        {
            // Get active role IDs of the user
            var userRoleIds = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            var activeRoleIds = await _context.Roles
                .Where(r => userRoleIds.Contains(r.Id) && r.IsActive)
                .Select(r => r.Id)
                .ToListAsync();

            // Check: permission exists via roles
            var hasViaRole = await _context.RolePermissions
                .AnyAsync(rp => activeRoleIds.Contains(rp.RoleId)
                    && _context.Permissions
                        .Any(p => p.Id == rp.PermissionId && p.Code == permissionCode && p.IsActive));

            if (hasViaRole) return true;

            // Check: permission exists via direct user-permission assignment
            var hasDirectly = await _context.UserPermissions
                .AnyAsync(up => up.UserId == userId
                    && _context.Permissions
                        .Any(p => p.Id == up.PermissionId && p.Code == permissionCode && p.IsActive));

            return hasDirectly;
        }
    }
}