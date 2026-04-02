using TicketApi.Modules.Identity.DTOs;

namespace TicketApi.Modules.Identity.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDto>> GetAllAsync();
        Task<PermissionDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreatePermissionDto request);
        Task UpdateAsync(int id, UpdatePermissionDto request);
        Task UpdateStatusAsync(int id, UpdatePermissionStatusDto request);
        Task DeleteAsync(int id);
    }
}
