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

        public User? GetByEmail(string email) => DbSet.FirstOrDefault(u => u.Email == email);
    }
}