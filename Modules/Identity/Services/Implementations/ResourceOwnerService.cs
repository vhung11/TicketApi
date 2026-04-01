using TicketApi.Modules.Identity.Repositories.Interfaces;
using TicketApi.Modules.Identity.Services.Interfaces;
using TicketApi.Modules.Orders.Repositories.Interfaces;

namespace TicketApi.Modules.Identity.Services.Implementations
{
    public class ResourceOwnerService : IResourceOwnerService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IOrderRepository _orderRepository;
        private const string AdminRoleName = "Admin";

        public ResourceOwnerService(
            IRoleRepository roleRepository,
            IOrderRepository orderRepository)
        {
            _roleRepository = roleRepository;
            _orderRepository = orderRepository;
        }

        public bool IsAdmin(int userId)
        {
            return _roleRepository.IsUserInRole(userId, AdminRoleName);
        }

        public bool IsOwner(int userId, string resourceType, int resourceId)
        {
            // Admin bypasses ownership check for all resource types
            if (IsAdmin(userId))
            {
                return true;
            }

            return resourceType switch
            {
                "Order" => _orderRepository.IsOwnedByUser(resourceId, userId),
                _ => false
            };
        }
    }
}
