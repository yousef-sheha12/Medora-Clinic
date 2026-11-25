using MedoraClinic.Application.Interfaces.IRepos;
using MedoraClinic.Domain.Entities;
using MedoraClinic.Domain.Enums;
using MedoraClinic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedoraClinic.Infrastructure.Repositories;

public sealed class AuthRepository : IAuthRepository
{
    private readonly AppDbContext context;

    public AuthRepository(AppDbContext context) => this.context = context;


    public Task<User?> GetUserByIdAsync(int userId,CancellationToken cancellationToken = default) =>
        context.Users.SingleOrDefaultAsync(user => user.Id == userId, cancellationToken);
    

    public Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        context.Users.SingleOrDefaultAsync(user => user.Email == email, cancellationToken);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default) =>
         await context.Users.AnyAsync(user => user.Email == email,cancellationToken);


    public async Task<bool> SpecialityExistsAsync(int specialityId, CancellationToken cancellationToken = default) =>
        await context.Specialties.AnyAsync(speciality => speciality.Id == specialityId,cancellationToken);


    public Task<VerificationCode?> GetVerificationCodeAsync(string tokenHash, VerificationType type, CancellationToken cancellationToken = default) =>
        context.VerificationCodes
            .Include(code => code.User)
            .SingleOrDefaultAsync(
                code => code.TokenHash == tokenHash
                    && code.Type == type
                    && code.UsedAt == null
                    && code.ExpiresAt > DateTime.UtcNow,
                cancellationToken);


    public Task<RefreshToken?> GetRefreshTokenAsync(string tokenHash, CancellationToken cancellationToken = default) =>
        context.RefreshTokens
            .Include(token => token.User)
            .SingleOrDefaultAsync(
                token => token.TokenHash == tokenHash
                    && token.RevokedAt == null
                    && token.ExpiresAt > DateTime.UtcNow,
                cancellationToken);


    public async Task AddUserAsync(User user, CancellationToken cancellationToken = default) =>
        await context.Users.AddAsync(user, cancellationToken);
    

    public async Task AddVerificationCodeAsync(VerificationCode code,CancellationToken cancellationToken = default) =>
         await context.VerificationCodes.AddAsync(code, cancellationToken);
    

    public async Task AddRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken = default) =>
        await context.RefreshTokens.AddAsync(token, cancellationToken);


    public async Task RevokeActiveRefreshTokensAsync(int userId, DateTime revokedAt, CancellationToken cancellationToken = default)
    {
        var tokens = await context.RefreshTokens
            .Where(token => token.UserId == userId && token.RevokedAt == null)
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
            token.RevokedAt = revokedAt;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);
    
}
