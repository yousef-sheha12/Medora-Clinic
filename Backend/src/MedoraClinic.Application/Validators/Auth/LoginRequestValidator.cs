using FluentValidation;
using MedoraClinic.Application.DTOs.Auth;

namespace MedoraClinic.Application.Validators.Auth;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("البريد الإلكتروني مطلوب.")
            .EmailAddress().WithMessage("صيغة البريد الإلكتروني غير صحيحة.");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("كلمة المرور مطلوبة.");
    }
}
