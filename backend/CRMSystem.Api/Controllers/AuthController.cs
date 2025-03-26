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
        var existingUser = await _userService.GetAsync(u => u.Username == user.Username);
        if (existingUser == null) {
            return Unauthorized();
        }

        var hashedPassword = _authService.HashPassword(user.Password);
        if (existingUser.Password != hashedPassword) {
            return Unauthorized();
        }

        var token = _authService.GenerateJwtToken(user.Username);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto user) {
        var existingUser = await _userService.GetAsync(u => u.Username == user.Username);
        if (existingUser != null) {
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
        return Ok(new { message = "User registered successfully" });
    }
}