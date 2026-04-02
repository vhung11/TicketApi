using TicketApi.Modules.Identity.DTOs;
using TicketApi.Modules.Identity.Entities;
using TicketApi.Modules.Identity.Extensions;
using TicketApi.Modules.Identity.Repositories.Interfaces;
using TicketApi.Modules.Identity.Services.Interfaces;

namespace TicketApi.Modules.Identity.Services.Implementations
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<PermissionDto>> GetAllAsync()
        {
            var permissions = await _permissionRepository.GetAllAsync();
            return permissions.Select(p => p.ToDto());
        }

        public async Task<PermissionDto> GetByIdAsync(int id)
        {
            var permission = await _permissionRepository.GetPermissionOrThrowAsync(id);
            return permission.ToDto();
        }

        public async Task<int> CreateAsync(CreatePermissionDto request)
        {
            var exists = await _permissionRepository.GetByCodeAsync(request.Code);
            if (exists != null)
            {
                throw new InvalidOperationException("Permission already exists");
            }
            var permission = new Permission
            {
                Code = request.Code,
                Resource = request.Resource,
                IsActive = true
            };
            _permissionRepository.Add(permission);
            return permission.Id;
        }

        public async Task UpdateAsync(int id, UpdatePermissionDto request)
        {
            var permission = await _permissionRepository.GetPermissionOrThrowAsync(id);
            permission.Code = request.Code;
            permission.Resource = request.Resource;
            _permissionRepository.Update(permission);
        }

        public async Task UpdateStatusAsync(int id, UpdatePermissionStatusDto request)
        {
            var permission = await _permissionRepository.GetPermissionOrThrowAsync(id);
            permission.IsActive = request.IsActive;
            _permissionRepository.Update(permission);
        }

        public async Task DeleteAsync(int id)
        {
            var permission = await _permissionRepository.GetPermissionOrThrowAsync(id);
            _permissionRepository.Delete(permission);
        }

    }
}