using Microsoft.AspNetCore.Mvc;
using TicketApi.Modules.Identity.Authorization;
using TicketApi.Modules.Identity.DTOs;
using TicketApi.Modules.Identity.Services.Interfaces;

namespace TicketApi.Modules.Identity.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/users
        [HttpGet]
        [HasPermission(Permissions.Users.Read)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // GET api/users/{id}
        [HttpGet("{id:int}")]
        [HasPermission(Permissions.Users.Read)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        // PUT api/users/{id}
        [HttpPut("{id:int}")]
        [HasPermission(Permissions.Users.Write)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto request)
        {
            await _userService.UpdateAsync(id, request);
            return NoContent();
        }

        // PATCH api/users/{id}/status
        [HttpPatch("{id:int}/status")]
        [HasPermission(Permissions.Users.Write)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateUserStatusDto request)
        {
            await _userService.UpdateStatusAsync(id, request);
            return NoContent();
        }

        // GET api/users/{id}/roles
        [HttpGet("{id:int}/roles")]
        [HasPermission(Permissions.Users.Read)]
        public async Task<IActionResult> GetRoles(int id)
        {
            var roles = await _userService.GetRolesAsync(id);
            return Ok(roles);
        }

        // POST api/users/{userId}/roles/{roleId}
        [HttpPost("{userId:int}/roles/{roleId:int}")]
        [HasPermission(Permissions.Users.Write)]
        public async Task<IActionResult> AssignRole(int userId, int roleId)
        {
            await _userService.AssignRoleAsync(userId, roleId);
            return NoContent();
        }

        // DELETE api/users/{userId}/roles/{roleId}
        [HttpDelete("{userId:int}/roles/{roleId:int}")]
        [HasPermission(Permissions.Users.Write)]
        public async Task<IActionResult> RemoveRole(int userId, int roleId)
        {
            await _userService.RemoveRoleAsync(userId, roleId);
            return NoContent();
        }

        // GET api/users/{userId}/permissions
        [HttpGet("{userId:int}/permissions")]
        [HasPermission(Permissions.Users.Read)]
        public async Task<IActionResult> GetPermissions(int userId)
        {
            var permissions = await _userService.GetPermissionsAsync(userId);
            return Ok(permissions);
        }
    }
}