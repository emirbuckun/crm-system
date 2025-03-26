using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CRMSystem.Api.Services;

public interface IAuthService {
    string HashPassword(string password);
    string GenerateJwtToken(string username);
}

public class AuthService(IConfiguration configuration) : IAuthService {
    private readonly IConfigurationSection _authConfig = configuration.GetSection("Authentication");

    public string HashPassword(string password) {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }

    public string GenerateJwtToken(string username) {
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfig["IssuerSigningKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _authConfig["ValidIssuer"],
            audience: _authConfig["ValidAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
