using TicketApi.Modules.Identity.DTOs;

namespace TicketApi.Modules.Identity.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task<RoleDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateRoleDto request);
        Task UpdateAsync(int id, UpdateRoleDto request);
        Task UpdateStatusAsync(int id, UpdateRoleStatusDto request);
        Task DeleteAsync(int id);
        Task<IEnumerable<PermissionDto>> GetPermissionsAsync(int roleId);
        Task AssignPermissionAsync(int roleId, int permissionId);
        Task RemovePermissionAsync(int roleId, int permissionId);
    }
}