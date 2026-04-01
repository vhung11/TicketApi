namespace TicketApi.Modules.Orders.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        /// <summary>
        /// Checks if the given order belongs to the specified user.
        /// </summary>
        bool IsOwnedByUser(int orderId, int userId);
    }
}
