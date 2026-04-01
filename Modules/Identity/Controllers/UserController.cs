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

        // GET api/user
        [HttpGet]
        [HasPermission(Permissions.Users.Read)]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        // GET api/user/{id}
        [HttpGet("{id:int}")]
        [HasPermission(Permissions.Users.Read)]
        public IActionResult GetById(int id)
        {
            try
            {
                var user = _userService.GetById(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // PUT api/user/{id}
        [HttpPut("{id:int}")]
        [HasPermission(Permissions.Users.Write)]
        public IActionResult Update(int id, [FromBody] UpdateUserDto request)
        {
            try
            {
                _userService.Update(id, request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PATCH api/user/{id}/status
        [HttpPatch("{id:int}/status")]
        [HasPermission(Permissions.Users.Write)]
        public IActionResult UpdateStatus(int id, [FromBody] UpdateUserStatusDto request)
        {
            try
            {
                _userService.UpdateStatus(id, request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET api/user/{id}/roles
        [HttpGet("{id:int}/roles")]
        [HasPermission(Permissions.Users.Read)]
        public IActionResult GetRoles(int id)
        {
            try
            {
                var roles = _userService.GetRoles(id);
                return Ok(roles);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // PUT api/user/{userId}/roles/{roleId}
        [HttpPut("{userId:int}/roles/{roleId:int}")]
        [HasPermission(Permissions.Users.Write)]
        public IActionResult AssignRole(int userId, int roleId)
        {
            try
            {
                _userService.AssignRole(userId, roleId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // GET api/user/{userId}/permissions
        [HttpGet("{userId:int}/permissions")]
        [HasPermission(Permissions.Users.Read)]
        public IActionResult GetPermissions(int userId)
        {
            try
            {
                var permissions = _userService.GetPermissions(userId);
                return Ok(permissions);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}