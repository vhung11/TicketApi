namespace TicketApi.Modules.Orders.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        /// <summary>
        /// Checks if the given order belongs to the specified user.
        /// </summary>
        Task<bool> IsOwnedByUserAsync(int orderId, int userId);
    }
}
