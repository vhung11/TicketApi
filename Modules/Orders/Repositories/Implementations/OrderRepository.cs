using Microsoft.EntityFrameworkCore;
using TicketApi.Infrastructure.Context;
using TicketApi.Modules.Orders.Repositories.Interfaces;

namespace TicketApi.Modules.Orders.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsOwnedByUserAsync(int orderId, int userId)
        {
            return await _context.Orders.AnyAsync(o => o.Id == orderId && o.UserId == userId);
        }
    }
}
