namespace TicketApi.Modules.Identity.Services.Interfaces
{
    public interface IResourceOwnerService
    {
        /// <summary>
        /// Returns true if the user is Admin (bypasses all ownership checks).
        /// </summary>
        Task<bool> IsAdminAsync(int userId);

        /// <summary>
        /// Returns true if the user owns the specified resource.
        /// Admin users always return true.
        /// </summary>
        Task<bool> IsOwnerAsync(int userId, string resourceType, int resourceId);
    }
}
