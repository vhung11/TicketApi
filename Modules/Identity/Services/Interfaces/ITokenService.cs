using TicketApi.Modules.Identity.Entities;

namespace TicketApi.Modules.Identity.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}