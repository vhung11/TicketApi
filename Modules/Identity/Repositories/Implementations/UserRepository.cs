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

        public async Task<User?> GetByEmailAsync(string email) => await DbSet.FirstOrDefaultAsync(u => u.Email == email);
    }
}