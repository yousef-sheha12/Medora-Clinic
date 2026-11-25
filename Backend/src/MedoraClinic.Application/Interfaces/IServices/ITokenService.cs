using MedoraClinic.Application.DTOs.Auth;
using MedoraClinic.Domain.Entities;

namespace MedoraClinic.Application.Interfaces.IServices
{
    public interface ITokenService
    {
        TokenResponse CreateToken(User user);
        RefreshToken CreateRefreshToken(int userId, string rawToken);
        string HashToken(string token);
    }
}
