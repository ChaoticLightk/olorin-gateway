using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Repositories.Interfaces;
using Gateway.Application.Authentication.Services.Interfaces;
using Gateway.Configuration.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.Application.Authentication.Services;

public class AuthenticationService(
    IOptions<JWTSettings> options,
    IApplicationRepository repository) : IAuthenticationService
{
    private readonly JWTSettings _jwtSettings = options.Value;

    public async Task<string?> AuthorizeApplication(string application, string password)
    {
        var app = await repository.GetApplicationDocumentAsync(application);

        if (app is null)
        {
            return null;
        }

        var computed = ComputeSha256Hash(password, app.Salt);

        if (!computed.Equals(app.Password))
        {
            return null;
        }

        return GenerateJwtToken(app.Name);
    }

    private static string ComputeSha256Hash(string password, string salt)
    {
        var combined = Encoding.UTF8.GetBytes(password + salt);
        var hashBytes = SHA256.HashData(combined);

        return Convert
            .ToHexString(hashBytes)
            .ToLowerInvariant();
    }

    private string GenerateJwtToken(string subject)
    {
        var secret = _jwtSettings.Key;

        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new InvalidOperationException("JWT secret key not configured.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
