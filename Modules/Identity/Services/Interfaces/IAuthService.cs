using TicketApi.Modules.Identity.DTOs;

namespace TicketApi.Modules.Identity.Services.Interfaces
{
    public interface IAuthService
    {
        void Register(RegisterRequestDto request);
        AuthResponseDto Login(LoginRequestDto request);
        UserDto GetCurrentUser(int userId);
    }
}
