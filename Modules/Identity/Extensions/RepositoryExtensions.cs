using TicketApi.Modules.Identity.Entities;
using TicketApi.Modules.Identity.Repositories.Interfaces;

namespace TicketApi.Modules.Identity.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<User> GetUserOrThrowAsync(this IUserRepository repository, int userId) =>
            await repository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException($"User with id {userId} not found.");

        public static async Task<Role> GetRoleOrThrowAsync(this IRoleRepository repository, int roleId) =>
            await repository.GetByIdAsync(roleId)
                ?? throw new KeyNotFoundException($"Role with id {roleId} not found.");

        public static async Task<Permission> GetPermissionOrThrowAsync(this IPermissionRepository repository, int permissionId) =>
            await repository.GetByIdAsync(permissionId)
                ?? throw new KeyNotFoundException($"Permission with id {permissionId} not found.");
    }
}
