using MedoraClinic.Application.DTOs.Auth;
using MedoraClinic.Application.Interfaces.IServices;
using MedoraClinic.Domain.Entities;
using MedoraClinic.Infrastructure.Services.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MedoraClinic.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions options;
        public TokenService(IOptions<JwtOptions> options) => this.options = options.Value;
   
        public TokenResponse CreateToken(User user)
        {
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(options.AccessTokenMinutes);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(options.Key));

            var credentials = new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler()
                .WriteToken(jwt);

            var refreshToken = Convert.ToBase64String(
                    RandomNumberGenerator.GetBytes(64))
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresInSeconds = (int)TimeSpan.FromMinutes(options.AccessTokenMinutes).TotalSeconds
            };
        }

        public string HashToken(string token)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(hash);
        }

        public RefreshToken CreateRefreshToken(int userId, string rawToken)
        {
            var now = DateTime.UtcNow;

            return new RefreshToken
            {
                UserId = userId,
                TokenHash = HashToken(rawToken),
                CreatedAt = now,
                ExpiresAt = now.AddDays(options.RefreshTokenDays)
            };
        }
    }
}
