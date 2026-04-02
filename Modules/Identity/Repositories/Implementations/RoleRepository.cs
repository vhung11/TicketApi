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

        public async Task<Role?> GetByNameAsync(string name) => await DbSet.FirstOrDefaultAsync(r => r.Name == name);

        public async Task<List<Role>> GetRolesAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role)
                .ToListAsync();
        }

        public async Task<bool> HasRoleAsync(int userId, int roleId)
        {
            return await _context.UserRoles
                .AnyAsync(ur => ur.UserId == userId
                    && ur.RoleId == roleId);
        }

        public async Task AssignRoleAsync(int userId, int roleId)
        {
            await _context.UserRoles.AddAsync(new UserRole { UserId = userId, RoleId = roleId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRoleAsync(int userId, int roleId)
        {
            _context.UserRoles.Remove(new UserRole { UserId = userId, RoleId = roleId });
            await _context.SaveChangesAsync();
        }
    }
}