using TicketApi.Modules.Identity.Repositories.Implementations;
using TicketApi.Modules.Identity.Repositories.Interfaces;
using TicketApi.Modules.Identity.Services.Implementations;
using TicketApi.Modules.Identity.Services.Interfaces;

namespace TicketApi.Modules.Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IResourceOwnerService, ResourceOwnerService>();

            return services;
        }
    }
}