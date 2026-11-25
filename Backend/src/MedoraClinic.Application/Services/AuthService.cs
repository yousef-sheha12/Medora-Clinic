using System.Security.Cryptography;
using MedoraClinic.Application.Common;
using MedoraClinic.Application.DTOs.Auth;
using MedoraClinic.Application.Interfaces.IRepos;
using MedoraClinic.Application.Interfaces.IServices;
using MedoraClinic.Domain.Entities;
using MedoraClinic.Domain.Enums;

namespace MedoraClinic.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IAuthRepository _repository;
    private readonly IEmailService _emailService;
    private readonly IPasswordHasherService _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        IAuthRepository repository,
        IEmailService emailService,
        IPasswordHasherService passwordHasher,
        ITokenService tokenService)
    {
        _repository = repository;
        _emailService = emailService;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<ApiResponse<object>> RegisterPatientAsync(RegisterPatientDto dto,CancellationToken cancellationToken = default)
    {
        if (!IsValidPassword(dto.Password))
            return Failure("كلمة المرور يجب أن تكون 10 أحرف على الأقل وبها حرف كبير وصغير ورقم.");

        if (dto.DateOfBirth.Date > DateTime.UtcNow.Date)
            return Failure("تاريخ الميلاد غير صحيح.");

        var email = NormalizeEmail(dto.Email);
        if (await _repository.EmailExistsAsync(email, cancellationToken))
            return Failure("البريد الإلكتروني مستخدم بالفعل.", 409);

        var user = new User
        {
            FullName = dto.FullName.Trim(),
            Email = email,
            Role = Role.Patient,
            IsEmailVerified = false,
            IsBlocked = false
        };

        user.PasswordHash = _passwordHasher.Hash(user, dto.Password);
        user.PatientProfile = new PatientProfile
        {
            User = user,
            ContactNumber = dto.ContactNumber.Trim(),
            Address = dto.Address.Trim(),
            DateOfBirth = dto.DateOfBirth,
            IsApproved = false
        };

        return await CreateUserAndSendVerificationAsync(user, cancellationToken);
    }

    public async Task<ApiResponse<object>> RegisterDoctorAsync(RegisterDoctorDto dto,CancellationToken cancellationToken = default)
    {
        if (!IsValidPassword(dto.Password))
            return Failure("كلمة المرور يجب أن تكون 10 أحرف على الأقل وبها حرف كبير وصغير ورقم.");

        if (dto.Price <= 0)
            return Failure("السعر يجب أن يكون أكبر من صفر.");

        if (!await _repository.SpecialityExistsAsync(
                dto.SpecialityId,
                cancellationToken))
            return Failure("التخصص غير موجود.", 404);

        var email = NormalizeEmail(dto.Email);
        if (await _repository.EmailExistsAsync(email, cancellationToken))
            return Failure("البريد الإلكتروني مستخدم بالفعل.", 409);

        var user = new User
        {
            FullName = dto.FullName.Trim(),
            Email = email,
            Role = Role.Doctor,
            IsEmailVerified = false,
            IsBlocked = false
        };

        user.PasswordHash = _passwordHasher.Hash(user, dto.Password);
        user.DoctorProfile = new DoctorProfile
        {
            User = user,
            ContactNumber = dto.ContactNumber.Trim(),
            Bio = dto.Bio.Trim(),
            Price = dto.Price,
            SpecialityId = dto.SpecialityId,
            IsApproved = false
        };

        return await CreateUserAndSendVerificationAsync(user, cancellationToken);
    }

    public async Task<ApiResponse<object>> VerifyEmailAsync(VerifyEmailRequest request,CancellationToken cancellationToken = default)
    {
        var code = await _repository.GetVerificationCodeAsync(
            _tokenService.HashToken(request.Token),
            VerificationType.EmailVerification,
            cancellationToken);

        if (code?.User is null)
            return Failure("الرابط غير صالح أو انتهت صلاحيته.", 400);

        code.UsedAt = DateTime.UtcNow;
        code.User.IsEmailVerified = true;
        await _repository.SaveChangesAsync(cancellationToken);

        return ApiResponse<object>.Success(message: "تم تأكيد البريد الإلكتروني.");
    }

    public async Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request,CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetUserByEmailAsync(
            NormalizeEmail(request.Email),
            cancellationToken);

        if (user is null
            || user.IsBlocked
            || !_passwordHasher.Verify(user, user.PasswordHash, request.Password))
        {
            return ApiResponse<TokenResponse>.Failure(
                "بيانات تسجيل الدخول غير صحيحة.",
                statusCode: 401);
        }

        if (!user.IsEmailVerified)
        {
            return ApiResponse<TokenResponse>.Failure(
                "أكد بريدك الإلكتروني قبل تسجيل الدخول.",
                statusCode: 403);
        }

        var tokens = _tokenService.CreateToken(user);
        await _repository.AddRefreshTokenAsync(
            _tokenService.CreateRefreshToken(user.Id, tokens.RefreshToken),
            cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return ApiResponse<TokenResponse>.Success(tokens, "تم تسجيل الدخول بنجاح.");
    }

    public async Task<ApiResponse<TokenResponse>> RefreshAsync(RefreshTokenRequest request,CancellationToken cancellationToken = default)
    {
        var storedToken = await _repository.GetRefreshTokenAsync(
            _tokenService.HashToken(request.RefreshToken),
            cancellationToken);

        if (storedToken?.User is null
            || storedToken.User.IsBlocked
            || !storedToken.User.IsEmailVerified)
        {
            return ApiResponse<TokenResponse>.Failure(
                "Refresh token غير صالح أو منتهي الصلاحية.",
                statusCode: 401);
        }

        storedToken.RevokedAt = DateTime.UtcNow;
        var tokens = _tokenService.CreateToken(storedToken.User);

        await _repository.AddRefreshTokenAsync(
            _tokenService.CreateRefreshToken(storedToken.UserId, tokens.RefreshToken),
            cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return ApiResponse<TokenResponse>.Success(tokens, "تم تجديد الجلسة.");
    }

    public async Task<ApiResponse<object>> LogoutAsync(RefreshTokenRequest request,CancellationToken cancellationToken = default)
    {
        var storedToken = await _repository.GetRefreshTokenAsync(
            _tokenService.HashToken(request.RefreshToken),
            cancellationToken);

        if (storedToken is not null)
        {
            storedToken.RevokedAt = DateTime.UtcNow;
            await _repository.SaveChangesAsync(cancellationToken);
        }

        return ApiResponse<object>.Success(message: "تم تسجيل الخروج.");
    }

    public async Task<ApiResponse<object>> ForgotPasswordAsync(ForgotPasswordRequest request,CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetUserByEmailAsync(
            NormalizeEmail(request.Email),
            cancellationToken);

        if (user is not null && !user.IsBlocked)
        {
            var rawToken = CreateRandomToken();

            await _repository.AddVerificationCodeAsync(
                new VerificationCode
                {
                    UserId = user.Id,
                    Type = VerificationType.PasswordReset,
                    TokenHash = _tokenService.HashToken(rawToken),
                    ExpiresAt = DateTime.UtcNow.AddMinutes(20)
                },
                cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            await _emailService.SendPasswordResetAsync(
                user.Email,
                rawToken);
        }

        return ApiResponse<object>.Success(
            message: "لو البريد مسجل، هتوصلك رسالة لاستعادة كلمة المرور.");
    }

    public async Task<ApiResponse<object>> ResetPasswordAsync(ResetPasswordRequest request,CancellationToken cancellationToken = default)
    {
        if (!IsValidPassword(request.NewPassword))
            return Failure("كلمة المرور يجب أن تكون 10 أحرف على الأقل وبها حرف كبير وصغير ورقم.");

        var code = await _repository.GetVerificationCodeAsync(
            _tokenService.HashToken(request.Token),
            VerificationType.PasswordReset,
            cancellationToken);

        if (code?.User is null || code.User.IsBlocked)
            return Failure("الرابط غير صالح أو انتهت صلاحيته.");

        code.UsedAt = DateTime.UtcNow;
        code.User.PasswordHash = _passwordHasher.Hash(
            code.User,
            request.NewPassword);

        await _repository.RevokeActiveRefreshTokensAsync(
            code.UserId,
            DateTime.UtcNow,
            cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return ApiResponse<object>.Success(message: "تم تغيير كلمة المرور.");
    }

    private async Task<ApiResponse<object>> CreateUserAndSendVerificationAsync(User user,CancellationToken cancellationToken)
    {
        var rawToken = CreateRandomToken();

        await _repository.AddUserAsync(user, cancellationToken);
        await _repository.AddVerificationCodeAsync(
            new VerificationCode
            {
                User = user,
                Type = VerificationType.EmailVerification,
                TokenHash = _tokenService.HashToken(rawToken),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            },
            cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);

        await _emailService.SendEmailVerificationAsync(user.Email, rawToken);

        return ApiResponse<object>.Success(
            message: "تم التسجيل. راجع بريدك لتأكيد الحساب.",
            statusCode: 202);
    }

    private static ApiResponse<object> Failure(string message, int statusCode = 400)
        => ApiResponse<object>.Failure(message, statusCode: statusCode);

    private static string NormalizeEmail(string email)
        => email.Trim().ToLowerInvariant();

    private static bool IsValidPassword(string password)
        => !string.IsNullOrWhiteSpace(password)
            && password.Length >= 10
            && password.Any(char.IsUpper)
            && password.Any(char.IsLower)
            && password.Any(char.IsDigit);

    private static string CreateRandomToken() =>
         Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    
}
