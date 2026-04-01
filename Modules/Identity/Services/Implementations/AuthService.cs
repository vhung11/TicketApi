using TicketApi.Modules.Identity.DTOs;
using TicketApi.Modules.Identity.Entities;
using TicketApi.Modules.Identity.Repositories.Interfaces;
using TicketApi.Modules.Identity.Services.Interfaces;

namespace TicketApi.Modules.Identity.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tokenService = tokenService;
        }

        public void Register(RegisterRequestDto request)
        {
            var existingUser = _userRepository.GetByEmail(request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            var defaultRole = _roleRepository.GetByName("User")
                ?? throw new InvalidOperationException("Default role 'User' not found. Run database seeder first.");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsActive = true
            };
            
            user.UserRoles.Add(new UserRole { RoleId = defaultRole.Id });
            _userRepository.Add(user);
            _userRepository.SaveChanges();
        }

        public AuthResponseDto Login(LoginRequestDto request)
        {
            var user = _userRepository.GetByEmail(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Account is disabled.");
            }

            var token = _tokenService.GenerateToken(user);
            return new AuthResponseDto { Token = token };
        }

        public UserDto GetCurrentUser(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return new UserDto
            {
                Id = user.Id,
                Username = user.Name,
                Email = user.Email,
                IsActive = user.IsActive
            };
        }
    }
}
