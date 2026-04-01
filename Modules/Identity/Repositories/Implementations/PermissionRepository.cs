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

        public Permission? GetByCode(string code) => DbSet.FirstOrDefault(p => p.Code == code);

        public IEnumerable<Permission> GetByResource(string resource) => DbSet.Where(p => p.Resource == resource).ToList();

        public bool UserHasPermission(int userId, string permissionCode)
        {
            // Get active role IDs of the user
            var userRoleIds = _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToList();

            var activeRoleIds = _context.Roles
                .Where(r => userRoleIds.Contains(r.Id) && r.IsActive)
                .Select(r => r.Id)
                .ToList();

            // Check: permission exists via roles
            var hasViaRole = _context.RolePermissions
                .Any(rp => activeRoleIds.Contains(rp.RoleId)
                    && _context.Permissions
                        .Any(p => p.Id == rp.PermissionId && p.Code == permissionCode && p.IsActive));

            if (hasViaRole) return true;

            // Check: permission exists via direct user-permission assignment
            var hasDirectly = _context.UserPermissions
                .Any(up => up.UserId == userId
                    && _context.Permissions
                        .Any(p => p.Id == up.PermissionId && p.Code == permissionCode && p.IsActive));

            return hasDirectly;
        }
    }
}