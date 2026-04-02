using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TicketApi.Modules.Identity.Services.Interfaces;

namespace TicketApi.Modules.Identity.Authorization
{
    /// <summary>
    /// Attribute to enforce permission-based authorization on controllers or actions.
    /// Queries the database on every request via IPermissionService.
    /// 
    /// Usage:
    ///   [HasPermission(Permissions.Orders.Create)]
    ///   [HasPermission(Permissions.Orders.Read, Permissions.Orders.Export)]  // requires ALL
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class HasPermissionAttribute : TypeFilterAttribute
    {
        public HasPermissionAttribute(params string[] permissions)
            : base(typeof(HasPermissionFilter))
        {
            Arguments = new object[] { permissions };
        }
    }

    public class HasPermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly string[] _requiredPermissions;
        private readonly IUserService _userService;

        public HasPermissionFilter(string[] permissions, IUserService userService)
        {
            _requiredPermissions = permissions;
            _userService = userService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userId = GetUserId(context.HttpContext.User);

            if (userId == null)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    statusCode = 401,
                    message = "Authentication is required."
                });
                return;
            }

            // Check that user has ALL required permissions
            foreach (var permissionCode in _requiredPermissions)
            {
                if (!await _userService.HasPermissionAsync(userId.Value, permissionCode))
                {
                    context.Result = new ObjectResult(new
                    {
                        statusCode = 403,
                        message = "You do not have permission to perform this action.",
                        requiredPermissions = _requiredPermissions
                    })
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                    return;
                }
            }
        }

        private static int? GetUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }

            return null;
        }
    }
}
