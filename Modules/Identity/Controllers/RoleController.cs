using Microsoft.AspNetCore.Mvc;
using TicketApi.Modules.Identity.Authorization;
using TicketApi.Modules.Identity.DTOs;
using TicketApi.Modules.Identity.Services.Interfaces;

namespace TicketApi.Modules.Identity.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET api/roles
        [HttpGet]
        [HasPermission(Permissions.Roles.Read)]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        // GET api/roles/{id}
        [HttpGet("{id:int}")]
        [HasPermission(Permissions.Roles.Read)]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            return Ok(role);
        }

        // POST api/roles
        [HttpPost]
        [HasPermission(Permissions.Roles.Write)]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto request)
        {
            var roleId = await _roleService.CreateAsync(request);
            return StatusCode(201, roleId);
        }

        // PUT api/roles/{id}
        [HttpPut("{id:int}")]
        [HasPermission(Permissions.Roles.Write)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleDto request)
        {
            await _roleService.UpdateAsync(id, request);
            return NoContent();
        }

        // PATCH api/roles/{id}/status
        [HttpPatch("{id:int}/status")]
        [HasPermission(Permissions.Roles.Write)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateRoleStatusDto request)
        {
            await _roleService.UpdateStatusAsync(id, request);
            return NoContent();
        }

        // DELETE api/roles/{id}
        [HttpDelete("{id:int}")]
        [HasPermission(Permissions.Roles.Write)]
        public async Task<IActionResult> Delete(int id)
        {
            await _roleService.DeleteAsync(id);
            return NoContent();
        }

        // GET api/roles/{id}/permissions
        [HttpGet("{id:int}/permissions")]
        [HasPermission(Permissions.Roles.Read)]
        public async Task<IActionResult> GetPermissions(int id)
        {
            var permissions = await _roleService.GetPermissionsAsync(id);
            return Ok(permissions);
        }

        // POST api/roles/{roleId}/permissions/{permissionId}
        [HttpPost("{roleId:int}/permissions/{permissionId:int}")]
        [HasPermission(Permissions.Roles.Write)]
        public async Task<IActionResult> AssignPermission(int roleId, int permissionId)
        {
            await _roleService.AssignPermissionAsync(roleId, permissionId);
            return NoContent();
        }

        // DELETE api/roles/{roleId}/permissions/{permissionId}
        [HttpDelete("{roleId:int}/permissions/{permissionId:int}")]
        [HasPermission(Permissions.Roles.Write)]
        public async Task<IActionResult> RemovePermission(int roleId, int permissionId)
        {
            await _roleService.RemovePermissionAsync(roleId, permissionId);
            return NoContent();
        }
    }
}