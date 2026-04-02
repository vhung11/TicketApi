using TicketApi.Modules.Identity.DTOs;
using TicketApi.Modules.Identity.Entities;
using TicketApi.Modules.Identity.Repositories.Interfaces;
using TicketApi.Modules.Identity.Services.Interfaces;

namespace TicketApi.Modules.Identity.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository
            )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }

        // ─── GET ALL ─────────────────────────────────────────────────────────────
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(ToDto);
        }

        // ─── GET BY ID ────────────────────────────────────────────────────────────
        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await GetUserOrThrowAsync(id);
            return ToDto(user);
        }

        // ─── UPDATE ───────────────────────────────────────────────────────────────
        public async Task UpdateAsync(int id, UpdateUserDto request)
        {
            var user = await GetUserOrThrowAsync(id);

            // Check email uniqueness if changed
            if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existing = await _userRepository.GetByEmailAsync(request.Email);
                if (existing != null)
                    throw new InvalidOperationException("Email is already in use by another account.");
            }

            user.Name = request.Name;
            user.Email = request.Email;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }

        // ─── UPDATE STATUS ────────────────────────────────────────────────────────
        public async Task UpdateStatusAsync(int id, UpdateUserStatusDto request)
        {
            var user = await GetUserOrThrowAsync(id);

            user.IsActive = request.IsActive;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoleDto>> GetRolesAsync(int userId)
        {
            await GetUserOrThrowAsync(userId);
            var roles = await _userRepository.GetRolesAsync(userId);
            return roles.Select(ToDto);
        }

        public async Task AssignRoleAsync(int userId, int roleId)
        {
            await GetUserOrThrowAsync(userId);
            await GetRoleOrThrowAsync(roleId);

            var existing = await _userRepository.GetUserRoleAsync(userId, roleId);
            if (existing != null)
                throw new InvalidOperationException("User is already assigned this role.");

            await _userRepository.AssignRoleAsync(userId, roleId);
            await _userRepository.SaveChangesAsync();
        }

        public async Task RemoveRoleAsync(int userId, int roleId)
        {
            await GetUserOrThrowAsync(userId);
            await GetRoleOrThrowAsync(roleId);

            _ = await _userRepository.GetUserRoleAsync(userId, roleId)
                ?? throw new InvalidOperationException("User is not assigned this role.");

            await _userRepository.RemoveRoleAsync(userId, roleId);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<PermissionDto>> GetPermissionsAsync(int userId)
        {
            await GetUserOrThrowAsync(userId);
            var permissions = await _userRepository.GetPermissionsAsync(userId);
            return permissions.Select(ToDto);
        }

        public async Task<bool> HasPermissionAsync(int userId, string permissionCode)
        {
            await GetUserOrThrowAsync(userId);
            return await _userRepository.HasPermissionAsync(userId, permissionCode);
        }

        public async Task AssignPermissionAsync(int userId, int permissionId)
        {
            await GetUserOrThrowAsync(userId);
            await GetPermissionOrThrowAsync(permissionId);

            var existing = await _userRepository.GetUserPermissionAsync(userId, permissionId);
            if (existing != null)
                throw new InvalidOperationException("User is already assigned this permission.");

            await _userRepository.AssignPermissionAsync(userId, permissionId);
            await _userRepository.SaveChangesAsync();
        }

        public async Task RemovePermissionAsync(int userId, int permissionId)
        {
            await GetUserOrThrowAsync(userId);
            await GetPermissionOrThrowAsync(permissionId);

            _ = await _userRepository.GetUserPermissionAsync(userId, permissionId)
                ?? throw new InvalidOperationException("User is not assigned this permission.");

            await _userRepository.RemovePermissionAsync(userId, permissionId);
            await _userRepository.SaveChangesAsync();
        }

        // ─── HELPERS ─────────────────────────────────────────────────────────
        private async Task<User> GetUserOrThrowAsync(int userId) =>
            await _userRepository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException($"User with id {userId} not found.");

        private async Task<Role> GetRoleOrThrowAsync(int roleId) =>
            await _roleRepository.GetByIdAsync(roleId)
                ?? throw new KeyNotFoundException($"Role with id {roleId} not found.");

        private async Task<Permission> GetPermissionOrThrowAsync(int permissionId) =>
            await _permissionRepository.GetByIdAsync(permissionId)
                ?? throw new KeyNotFoundException($"Permission with id {permissionId} not found.");

        private static UserDto ToDto(User u) => new()
        {
            Id = u.Id,
            Username = u.Name,
            Email = u.Email,
            IsActive = u.IsActive
        };

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
