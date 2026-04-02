using System.Security.Claims;
using TicketApi.Modules.Identity.DTOs;

namespace TicketApi.Modules.Identity.Services.Interfaces
{
    public interface IAuthService
    {
        Task<int> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<UserDto> GetCurrentUserAsync(ClaimsPrincipal claimsPrincipal);
    }
}
