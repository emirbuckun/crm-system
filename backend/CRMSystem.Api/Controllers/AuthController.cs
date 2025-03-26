using CRMSystem.Api.DTOs;
using CRMSystem.Api.Models;
using CRMSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService, IAuthService authService, ILogger<AuthController> logger) : ControllerBase {
    private readonly IUserService _userService = userService;
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto user) {
        try {
            _logger.LogInformation("Login attempt for user: {Username}", user.Username);
            var existingUser = await _userService.GetAsync(u => u.Username == user.Username);
            if (existingUser == null) {
                _logger.LogWarning("Login failed for user: {Username} - User not found", user.Username);
                return Unauthorized();
            }

            var hashedPassword = _authService.HashPassword(user.Password);
            if (existingUser.Password != hashedPassword) {
                _logger.LogWarning("Login failed for user: {Username} - Incorrect password", user.Username);
                return Unauthorized();
            }

            var token = _authService.GenerateJwtToken(user.Username);
            _logger.LogInformation("Login successful for user: {Username}", user.Username);
            return Ok(new { token });
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred during login for user: {Username}", user.Username);
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto user) {
        try {
            _logger.LogInformation("Registration attempt for user: {Username}", user.Username);
            var existingUser = await _userService.GetAsync(u => u.Username == user.Username);
            if (existingUser != null) {
                _logger.LogWarning("Registration failed for user: {Username} - Username already exists", user.Username);
                return BadRequest(new { message = "Username already exists" });
            }

            var hashedPassword = _authService.HashPassword(user.Password);

            var newUser = new User {
                Username = user.Username,
                Password = hashedPassword,
                Role = "user",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userService.CreateAsync(newUser);
            _logger.LogInformation("User registered successfully: {Username}", user.Username);
            return Ok(new { message = "User registered successfully" });
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred during registration for user: {Username}", user.Username);
            return StatusCode(500, new { message = "An error occurred during registration" });
        }
    }
}