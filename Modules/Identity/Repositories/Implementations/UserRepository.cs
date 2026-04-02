using Microsoft.EntityFrameworkCore;
using TicketApi.Infrastructure.Context;
using TicketApi.Modules.Identity.Entities;
using TicketApi.Modules.Identity.Repositories.Interfaces;

namespace TicketApi.Modules.Identity.Repositories.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        protected override DbSet<User> DbSet => _context.Users;

        public async Task<User?> GetByEmailAsync(string email) =>
            await DbSet.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<IEnumerable<Role>> GetRolesAsync(int userId) =>
            await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role)
                .ToListAsync();

        public async Task<IEnumerable<Permission>> GetPermissionsAsync(int userId) =>
            await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.Permission)
                .ToListAsync();

        public async Task<UserRole?> GetUserRoleAsync(int userId, int roleId) =>
            await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

        public async Task<UserPermission?> GetUserPermissionAsync(int userId, int permissionId) =>
            await _context.UserPermissions
                .FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permissionId);

        public async Task AssignRoleAsync(int userId, int roleId) =>
            await _context.UserRoles.AddAsync(new UserRole { UserId = userId, RoleId = roleId });

        public async Task RemoveRoleAsync(int userId, int roleId)
        {
            _context.UserRoles.Remove(new UserRole { UserId = userId, RoleId = roleId });
            await _context.SaveChangesAsync();
        }

        public async Task AssignPermissionAsync(int userId, int permissionId) =>
            await _context.UserPermissions.AddAsync(new UserPermission { UserId = userId, PermissionId = permissionId });

        public async Task RemovePermissionAsync(int userId, int permissionId)
        {
            _context.UserPermissions.Remove(new UserPermission { UserId = userId, PermissionId = permissionId });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasPermissionAsync(int userId, string permissionCode)
        {
            return await _context.UserPermissions
                .AnyAsync(up => up.UserId == userId && up.Permission.Code == permissionCode);
        }
    }
}