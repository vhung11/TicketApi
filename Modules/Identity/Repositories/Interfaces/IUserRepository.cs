using TicketApi.Modules.Identity.Entities;

namespace TicketApi.Modules.Identity.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User? GetByEmail(string email);
    }
}