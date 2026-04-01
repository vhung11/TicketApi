using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TicketApi.Modules.Identity.Services.Interfaces;

namespace TicketApi.Modules.Identity.Authorization
{
    /// <summary>
    /// Verifies that the authenticated user owns the resource being accessed.
    /// Admin users bypass this check automatically.
    ///
    /// Usage:
    ///   [CheckResourceOwner("Order")]                        // reads route param "id"
    ///   [CheckResourceOwner("Order", routeParam: "orderId")] // custom route param name
    ///
    /// Combine with [HasPermission] to enforce both permission and ownership:
    ///   [HasPermission(Permissions.Orders.Update)]
    ///   [CheckResourceOwner("Order")]
    ///   public ActionResult UpdateOrder(int id) { ... }
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CheckResourceOwnerAttribute : TypeFilterAttribute
    {
        public CheckResourceOwnerAttribute(string resourceType, string routeParam = "id")
            : base(typeof(CheckResourceOwnerFilter))
        {
            Arguments = new object[] { resourceType, routeParam };
        }
    }

    public class CheckResourceOwnerFilter : IAuthorizationFilter
    {
        private readonly string _resourceType;
        private readonly string _routeParam;
        private readonly IResourceOwnerService _resourceOwnerService;

        public CheckResourceOwnerFilter(
            string resourceType,
            string routeParam,
            IResourceOwnerService resourceOwnerService)
        {
            _resourceType = resourceType;
            _routeParam = routeParam;
            _resourceOwnerService = resourceOwnerService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
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

            // Extract resource ID from route
            var routeValue = context.RouteData.Values[_routeParam]?.ToString();
            if (!int.TryParse(routeValue, out var resourceId))
            {
                context.Result = new BadRequestObjectResult(new
                {
                    statusCode = 400,
                    message = $"Invalid or missing route parameter '{_routeParam}'."
                });
                return;
            }

            if (!_resourceOwnerService.IsOwner(userId.Value, _resourceType, resourceId))
            {
                context.Result = new ObjectResult(new
                {
                    statusCode = 403,
                    message = "You do not have permission to access this resource."
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
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
