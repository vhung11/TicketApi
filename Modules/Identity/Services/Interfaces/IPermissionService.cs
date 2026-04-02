namespace TicketApi.Modules.Identity.Services.Interfaces
{
    public interface IPermissionService
    {
        /// <summary>
        /// Checks if a user has a specific permission code.
        /// </summary>
        Task<bool> HasPermissionAsync(int userId, string permissionCode);
    }
}
