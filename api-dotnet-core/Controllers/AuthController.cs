using api_dotnet_core.Repositories;
using api_dotnet_core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using api_dotnet_core.DTOs;

namespace api_dotnet_core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<object> _pwHasher;


        public AuthController(IUserRepository repo, ITokenService tokenService, IPasswordHasher<object> pwHasher)
        {
            _repo = repo;
            _tokenService = tokenService;
            _pwHasher = pwHasher;
        }


        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignupDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            // Check if user exists
            var existing = await _repo.GetByEmailAsync(dto.Email);
            if (existing != null) return Conflict(new { message = "Email already registered." });


            // Hash password
            var hashed = _pwHasher.HashPassword(null, dto.Password);


            var user = new api_dotnet_core.DTOs.UserDto
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = hashed
            };


            var created = await _repo.CreateUserAsync(user);
            if (!created) return StatusCode(500, new { message = "Could not create user." });


            return Ok(new { message = "User created successfully." });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var user = await _repo.GetByEmailAsync(dto.Email);
            if (user == null) return Unauthorized(new { message = "Invalid credentials." });


            var result = _pwHasher.VerifyHashedPassword(null, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed) return Unauthorized(new { message = "Invalid credentials." });


            var token = _tokenService.GenerateToken(user);
            return Ok(new AuthResponseDto { Token = token, ExpiresInMinutes = int.Parse(System.Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES") ?? "60") });
        }
    }
}
