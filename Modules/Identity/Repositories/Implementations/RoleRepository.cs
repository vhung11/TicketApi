using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketApi.Infrastructure.Context;
using TicketApi.Modules.Identity.Entities;
using TicketApi.Modules.Identity.Repositories.Interfaces;

namespace TicketApi.Modules.Identity.Repositories.Implementations
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        protected override DbSet<Role> DbSet => _context.Roles;

        public async Task<Role?> GetByNameAsync(string name) =>
            await DbSet.FirstOrDefaultAsync(r => r.Name == name);

        public async Task<List<Permission>> GetPermissionsAsync(int roleId) =>
            await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToListAsync();

        public async Task AssignPermissionAsync(int roleId, int permissionId) =>
            await _context.RolePermissions.AddAsync(new RolePermission { RoleId = roleId, PermissionId = permissionId });

        public async Task RemovePermissionAsync(int roleId, int permissionId)
        {
            _context.RolePermissions.Remove(new RolePermission { RoleId = roleId, PermissionId = permissionId });
            await _context.SaveChangesAsync();
        }
    }
}