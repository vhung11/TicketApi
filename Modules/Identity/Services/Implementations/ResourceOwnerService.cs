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

        public async Task<bool> IsAdminAsync(int userId)
        {
            var roles = await _roleRepository.GetRolesAsync(userId);
            return roles.Any(r => r.Name == AdminRoleName);
        }

        public async Task<bool> IsOwnerAsync(int userId, string resourceType, int resourceId)
        {
            // Admin bypasses ownership check for all resource types
            if (await IsAdminAsync(userId))
            {
                return true;
            }

            return resourceType switch
            {
                "Order" => await _orderRepository.IsOwnedByUserAsync(resourceId, userId),
                _ => false
            };
        }
    }
}
