using Microsoft.AspNetCore.Mvc;
using TicketApi.Modules.Identity.Authorization;
using TicketApi.Modules.Identity.DTOs;
using TicketApi.Modules.Identity.Services.Interfaces;

namespace TicketApi.Modules.Identity.Controllers
{
    [ApiController]
    [Route("api/permissions")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        // GET api/permissions
        [HttpGet]
        [HasPermission(Permissions.PermissionMgmt.Read)]
        public async Task<IActionResult> GetAll()
        {
            var permissions = await _permissionService.GetAllAsync();
            return Ok(permissions);
        }

        // GET api/permissions/{id}
        [HttpGet("{id}")]
        [HasPermission(Permissions.PermissionMgmt.Read)]
        public async Task<IActionResult> GetById(int id)
        {
            var permission = await _permissionService.GetByIdAsync(id);
            return Ok(permission);
        }

        // POST api/permissions
        [HttpPost]
        [HasPermission(Permissions.PermissionMgmt.Write)]
        public async Task<IActionResult> Create([FromBody] CreatePermissionDto request)
        {
            var permissionId = await _permissionService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = permissionId }, null);
        }

        // PUT api/permissions/{id}
        [HttpPut("{id}")]
        [HasPermission(Permissions.PermissionMgmt.Write)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePermissionDto request)
        {
            await _permissionService.UpdateAsync(id, request);
            return NoContent();
        }

        // DELETE api/permissions/{id}
        [HttpDelete("{id}")]
        [HasPermission(Permissions.PermissionMgmt.Write)]
        public async Task<IActionResult> Delete(int id)
        {
            await _permissionService.DeleteAsync(id);
            return NoContent();
        }
    }
}