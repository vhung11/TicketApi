namespace TicketApi.Modules.Identity.Services.Interfaces
{
    public interface IPermissionService
    {
        /// <summary>
        /// Checks if a user has a specific permission code.
        /// </summary>
        bool HasPermission(int userId, string permissionCode);
    }
}
