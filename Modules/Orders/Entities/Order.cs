using TicketApi.Modules.Orders.Enums;

namespace TicketApi.Modules.Orders.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}