using Microsoft.EntityFrameworkCore;
using TicketApi.Infrastructure.Context;
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
        private readonly ApplicationDbContext _context;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _context = context;
        }

        // ─── GET ALL ─────────────────────────────────────────────────────────────
        public IEnumerable<UserDto> GetAll()
        {
            return _userRepository.GetAll().Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Name,
                Email = u.Email,
                IsActive = u.IsActive
            });
        }

        // ─── GET BY ID ────────────────────────────────────────────────────────────
        public UserDto GetById(int id)
        {
            var user = _userRepository.GetById(id)
                ?? throw new KeyNotFoundException($"User with id {id} not found.");

            return new UserDto
            {
                Id = user.Id,
                Username = user.Name,
                Email = user.Email,
                IsActive = user.IsActive
            };
        }

        // ─── UPDATE ───────────────────────────────────────────────────────────────
        public void Update(int id, UpdateUserDto request)
        {
            var user = _userRepository.GetById(id)
                ?? throw new KeyNotFoundException($"User with id {id} not found.");

            // Check email uniqueness if changed
            if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existing = _userRepository.GetByEmail(request.Email);
                if (existing != null)
                    throw new InvalidOperationException("Email is already in use by another account.");
            }

            user.Name = request.Name;
            user.Email = request.Email;

            _userRepository.Update(user);
            _userRepository.SaveChanges();
        }

        // ─── UPDATE STATUS ────────────────────────────────────────────────────────
        public void UpdateStatus(int id, UpdateUserStatusDto request)
        {
            var user = _userRepository.GetById(id)
                ?? throw new KeyNotFoundException($"User with id {id} not found.");

            user.IsActive = request.IsActive;

            _userRepository.Update(user);
            _userRepository.SaveChanges();
        }

        // ─── GET ROLES ────────────────────────────────────────────────────────────
        public IEnumerable<RoleDto> GetRoles(int userId)
        {
            _ = _userRepository.GetById(userId)
                ?? throw new KeyNotFoundException($"User with id {userId} not found.");

            return _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .Select(ur => new RoleDto
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name,
                    IsActive = ur.Role.IsActive
                })
                .ToList();
        }

        // ─── ASSIGN ROLE ──────────────────────────────────────────────────────────
        public void AssignRole(int userId, int roleId)
        {
            _ = _userRepository.GetById(userId)
                ?? throw new KeyNotFoundException($"User with id {userId} not found.");

            var role = _roleRepository.GetById(roleId)
                ?? throw new KeyNotFoundException($"Role with id {roleId} not found.");

            bool alreadyAssigned = _context.UserRoles
                .Any(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (alreadyAssigned)
                throw new InvalidOperationException($"User already has role '{role.Name}'.");

            _context.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
            _context.SaveChanges();
        }

        // ─── GET PERMISSIONS ──────────────────────────────────────────────────────
        public IEnumerable<PermissionDto> GetPermissions(int userId)
        {
            _ = _userRepository.GetById(userId)
                ?? throw new KeyNotFoundException($"User with id {userId} not found.");

            // Permissions via roles
            var roleIds = _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToList();

            var viaRoles = _context.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Include(rp => rp.Permissions)
                .Select(rp => rp.Permissions)
                .ToList();

            // Permissions directly assigned
            var directly = _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Include(up => up.Permissions)
                .Select(up => up.Permissions)
                .ToList();

            return viaRoles
                .Union(directly)
                .DistinctBy(p => p.Id)
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Code = p.Code,
                    Resource = p.Resource,
                    IsActive = p.IsActive
                })
                .ToList();
        }
    }
}
