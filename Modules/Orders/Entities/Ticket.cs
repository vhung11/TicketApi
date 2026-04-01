using TicketApi.Modules.Orders.Enums;

namespace TicketApi.Modules.Orders.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int ConcertId { get; set; }
        public TicketZone Zone { get; set; }
        public TicketStatus Status { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}