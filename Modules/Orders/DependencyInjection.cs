using TicketApi.Modules.Orders.Repositories.Implementations;
using TicketApi.Modules.Orders.Repositories.Interfaces;

namespace TicketApi.Modules.Orders
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOrdersModule(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}