using TicketApi.Modules.Identity.DTOs;
using TicketApi.Modules.Identity.Entities;
using TicketApi.Modules.Identity.Extensions;
using TicketApi.Modules.Identity.Repositories.Interfaces;
using TicketApi.Modules.Identity.Services.Interfaces;

namespace TicketApi.Modules.Identity.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;

        public RoleService(
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository
            )
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles.Select(r => r.ToDto());
        }

        public async Task<RoleDto> GetByIdAsync(int id)
        {
            var role = await _roleRepository.GetRoleOrThrowAsync(id);
            return role.ToDto();
        }

        public async Task<int> CreateAsync(CreateRoleDto request)
        {
            var exists = await _roleRepository.GetByNameAsync(request.Name);
            if (exists != null)
            {
                throw new InvalidOperationException("Role already exists");
            }
            var role = new Role
            {
                Name = request.Name,
                IsActive = true
            };
            _roleRepository.Add(role);
            return role.Id;
        }

        public async Task UpdateAsync(int id, UpdateRoleDto request)
        {
            var role = await _roleRepository.GetRoleOrThrowAsync(id);
            role.Name = request.Name;
            _roleRepository.Update(role);
        }

        public async Task UpdateStatusAsync(int id, UpdateRoleStatusDto request)
        {
            var role = await _roleRepository.GetRoleOrThrowAsync(id);
            role.IsActive = request.IsActive;
            _roleRepository.Update(role);
        }

        public async Task DeleteAsync(int id)
        {
            var role = await _roleRepository.GetRoleOrThrowAsync(id);
            _roleRepository.Delete(role);
        }

        public async Task<IEnumerable<PermissionDto>> GetPermissionsAsync(int roleId)
        {
            await _roleRepository.GetRoleOrThrowAsync(roleId);
            var permissions = await _roleRepository.GetPermissionsAsync(roleId);
            return permissions.Select(p => p.ToDto());
        }

        public async Task AssignPermissionAsync(int roleId, int permissionId)
        {
            await _roleRepository.GetRoleOrThrowAsync(roleId);
            await _permissionRepository.GetPermissionOrThrowAsync(permissionId);
            var permissions = await _roleRepository.GetPermissionsAsync(roleId);
            if (permissions.Any(p => p.Id == permissionId))
                throw new InvalidOperationException("Role is already assigned this permission.");
            await _roleRepository.AssignPermissionAsync(roleId, permissionId);
        }

        public async Task RemovePermissionAsync(int roleId, int permissionId)
        {
            await _roleRepository.GetRoleOrThrowAsync(roleId);
            await _permissionRepository.GetPermissionOrThrowAsync(permissionId);
            var permissions = await _roleRepository.GetPermissionsAsync(roleId);
            if (!permissions.Any(p => p.Id == permissionId))
                throw new InvalidOperationException("Role is not assigned this permission.");
            await _roleRepository.RemovePermissionAsync(roleId, permissionId);
        }

    }
}