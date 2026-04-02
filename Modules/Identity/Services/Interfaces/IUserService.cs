using TicketApi.Modules.Identity.DTOs;

namespace TicketApi.Modules.Identity.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>Lấy danh sách tất cả user.</summary>
        Task<IEnumerable<UserDto>> GetAllAsync();

        /// <summary>Lấy thông tin một user theo id.</summary>
        Task<UserDto> GetByIdAsync(int id);

        /// <summary>Cập nhật thông tin cơ bản của user (name, email).</summary>
        Task UpdateAsync(int id, UpdateUserDto request);

        /// <summary>Cập nhật trạng thái active/inactive của user.</summary>
        Task UpdateStatusAsync(int id, UpdateUserStatusDto request);

        /// <summary>Lấy danh sách tất cả role của user.</summary>
        Task<IEnumerable<RoleDto>> GetRolesAsync(int userId);

        /// <summary>Thêm một role cho user.</summary>
        Task AssignRoleAsync(int userId, int roleId);

        /// <summary>Xóa một role của user.</summary>
        Task RemoveRoleAsync(int userId, int roleId);

        /// <summary>Lấy danh sách tất cả permission của user.</summary>
        Task<IEnumerable<PermissionDto>> GetPermissionsAsync(int userId);

        /// <summary>
        /// Checks if a user has a specific permission code.
        /// </summary>
        Task<bool> HasPermissionAsync(int userId, string permissionCode);

        /// <summary>Thêm một permission cho user.</summary>
        Task AssignPermissionAsync(int userId, int permissionId);

        /// <summary>Xóa một permission của user.</summary>
        Task RemovePermissionAsync(int userId, int permissionId);
    }
}
