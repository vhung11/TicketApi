using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketApi.Modules.Identity.DTOs;
using TicketApi.Modules.Identity.Services.Interfaces;

namespace TicketApi.Modules.Identity.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var userId = await _authService.RegisterAsync(request);
            return StatusCode(201, new 
            { 
                Message = "User registered successfully", 
                UserId = userId, 
                request.Email 
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            return Ok(await _authService.GetCurrentUserAsync(User));
        }
    }
}
