using TicketApi.Modules.Identity.DTOs;
using TicketApi.Modules.Identity.Entities;
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
            return roles.Select(ToDto);
        }

        public async Task<RoleDto> GetByIdAsync(int id)
        {
            var role = await GetRoleOrThrowAsync(id);
            return ToDto(role);
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
            var role = await GetRoleOrThrowAsync(id);
            role.Name = request.Name;
            _roleRepository.Update(role);
        }

        public async Task UpdateStatusAsync(int id, UpdateRoleStatusDto request)
        {
            var role = await GetRoleOrThrowAsync(id);
            role.IsActive = request.IsActive;
            _roleRepository.Update(role);
        }

        public async Task DeleteAsync(int id)
        {
            var role = await GetRoleOrThrowAsync(id);
            _roleRepository.Delete(role);
        }

        public async Task<IEnumerable<PermissionDto>> GetPermissionsAsync(int roleId)
        {
            await GetRoleOrThrowAsync(roleId);
            var permissions = await _roleRepository.GetPermissionsAsync(roleId);
            return permissions.Select(ToDto);
        }

        public async Task AssignPermissionAsync(int roleId, int permissionId)
        {
            await GetRoleOrThrowAsync(roleId);
            await GetPermissionOrThrowAsync(permissionId);
            _ = await _roleRepository.GetPermissionsAsync(roleId)
                ?? throw new InvalidOperationException("Role is already assigned this permission.");
            await _roleRepository.AssignPermissionAsync(roleId, permissionId);
        }

        public async Task RemovePermissionAsync(int roleId, int permissionId)
        {
            await GetRoleOrThrowAsync(roleId);
            await GetPermissionOrThrowAsync(permissionId);
            _ = await _roleRepository.GetPermissionsAsync(roleId)
                ?? throw new InvalidOperationException("Role is not assigned this permission.");
            await _roleRepository.RemovePermissionAsync(roleId, permissionId);
        }

        private async Task<Role> GetRoleOrThrowAsync(int roleId) =>
            await _roleRepository.GetByIdAsync(roleId)
                ?? throw new KeyNotFoundException($"Role with id {roleId} not found.");

        private async Task<Permission> GetPermissionOrThrowAsync(int permissionId) =>
            await _permissionRepository.GetByIdAsync(permissionId)
                ?? throw new KeyNotFoundException($"Permission with id {permissionId} not found.");

        private static RoleDto ToDto(Role r) => new()
        {
            Id = r.Id,
            Name = r.Name,
            IsActive = r.IsActive
        };

        private static PermissionDto ToDto(Permission p) => new()
        {
            Id = p.Id,
            Code = p.Code,
            Resource = p.Resource,
            IsActive = p.IsActive
        };
    }
}