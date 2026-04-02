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
    }
}
