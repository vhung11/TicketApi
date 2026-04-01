using TicketApi.Modules.Identity.DTOs;

namespace TicketApi.Modules.Identity.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>Lấy danh sách tất cả user.</summary>
        IEnumerable<UserDto> GetAll();

        /// <summary>Lấy thông tin một user theo id.</summary>
        UserDto GetById(int id);

        /// <summary>Cập nhật thông tin cơ bản của user (name, email).</summary>
        void Update(int id, UpdateUserDto request);

        /// <summary>Cập nhật trạng thái active/inactive của user.</summary>
        void UpdateStatus(int id, UpdateUserStatusDto request);

        /// <summary>Lấy danh sách roles của user.</summary>
        IEnumerable<RoleDto> GetRoles(int userId);

        /// <summary>Gán một role cho user.</summary>
        void AssignRole(int userId, int roleId);

        /// <summary>Lấy danh sách permissions của user (via roles + direct).</summary>
        IEnumerable<PermissionDto> GetPermissions(int userId);
    }
}
