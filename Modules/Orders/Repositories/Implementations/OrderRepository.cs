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

        public bool IsOwnedByUser(int orderId, int userId)
        {
            return _context.Orders.Any(o => o.Id == orderId && o.UserId == userId);
        }
    }
}
