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

        public Role? GetByName(string name) => DbSet.FirstOrDefault(r => r.Name == name);

        public bool IsUserInRole(int userId, string roleName)
        {
            var role = DbSet.FirstOrDefault(r => r.Name == roleName && r.IsActive);
            if (role == null) return false;

            return _context.UserRoles.Any(ur => ur.UserId == userId && ur.RoleId == role.Id);
        }
    }
}