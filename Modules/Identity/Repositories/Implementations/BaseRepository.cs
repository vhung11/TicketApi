using Microsoft.EntityFrameworkCore;
using TicketApi.Infrastructure.Context;
using TicketApi.Modules.Identity.Repositories.Interfaces;

namespace TicketApi.Modules.Identity.Repositories.Implementations
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected abstract DbSet<T> DbSet { get; }

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(int id) => await DbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await DbSet.ToListAsync();

        public void Add(T entity) => DbSet.Add(entity);

        public void Update(T entity) => DbSet.Update(entity);

        public virtual void Delete(T entity) => DbSet.Remove(entity);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}