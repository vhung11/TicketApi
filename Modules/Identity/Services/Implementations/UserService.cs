using TicketApi.Modules.Identity.DTOs;
using TicketApi.Modules.Identity.Entities;
using TicketApi.Modules.Identity.Repositories.Interfaces;
using TicketApi.Modules.Identity.Services.Interfaces;

namespace TicketApi.Modules.Identity.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(
            IUserRepository userRepository
            )
        {
            _userRepository = userRepository;
        }

        // ─── GET ALL ─────────────────────────────────────────────────────────────
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Name,
                Email = u.Email,
                IsActive = u.IsActive
            });
        }

        // ─── GET BY ID ────────────────────────────────────────────────────────────
        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await GetUserOrThrowAsync(id);

            return new UserDto
            {
                Id = user.Id,
                Username = user.Name,
                Email = user.Email,
                IsActive = user.IsActive
            };
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

        // ─── HELPERS ─────────────────────────────────────────────────────────
        private async Task<User> GetUserOrThrowAsync(int userId) =>
            await _userRepository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException($"User with id {userId} not found.");
    }
}
