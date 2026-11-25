using MedoraClinic.Domain.Entities;
using MedoraClinic.Domain.Enums;

namespace MedoraClinic.Application.Interfaces.IRepos
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> SpecialityExistsAsync(int specialityId, CancellationToken cancellationToken = default);
        Task<VerificationCode?> GetVerificationCodeAsync(string tokenHash, VerificationType type, CancellationToken cancellationToken = default);
        Task<RefreshToken?> GetRefreshTokenAsync(string tokenHash, CancellationToken cancellationToken = default);
        Task AddUserAsync(User user, CancellationToken cancellationToken = default);
        Task AddVerificationCodeAsync(VerificationCode code, CancellationToken cancellationToken = default);
        Task AddRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken = default);
        Task RevokeActiveRefreshTokensAsync(int userId, DateTime revokedAt, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
