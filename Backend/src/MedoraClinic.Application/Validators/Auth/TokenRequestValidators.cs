using FluentValidation;
using MedoraClinic.Application.DTOs.Auth;

namespace MedoraClinic.Application.Validators.Auth;

public sealed class VerifyEmailRequestValidator
    : AbstractValidator<VerifyEmailRequest>
{
    public VerifyEmailRequestValidator()
    {
        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("رمز التفعيل مطلوب.");
    }
}

public sealed class RefreshTokenRequestValidator
    : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(request => request.RefreshToken)
            .NotEmpty().WithMessage("Refresh token مطلوب.");
    }
}
