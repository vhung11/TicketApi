namespace TicketApi.Modules.Orders.Entities
{
    public class Concert
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Venue { get; set; } = string.Empty;

        public ICollection<Ticket> Tickets { get; set; } = null!;
    }
}