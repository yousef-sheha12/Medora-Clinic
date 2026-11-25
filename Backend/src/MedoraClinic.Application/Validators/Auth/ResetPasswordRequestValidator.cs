using FluentValidation;
using MedoraClinic.Application.DTOs.Auth;

namespace MedoraClinic.Application.Validators.Auth;

public sealed class ResetPasswordRequestValidator
    : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("رمز إعادة التعيين مطلوب.");

        RuleFor(request => request.NewPassword)
            .NotEmpty().WithMessage("كلمة المرور الجديدة مطلوبة.")
            .MinimumLength(10).WithMessage("كلمة المرور يجب أن تكون 10 أحرف على الأقل.")
            .Matches("[A-Z]").WithMessage("كلمة المرور يجب أن تحتوي على حرف كبير.")
            .Matches("[a-z]").WithMessage("كلمة المرور يجب أن تحتوي على حرف صغير.")
            .Matches("[0-9]").WithMessage("كلمة المرور يجب أن تحتوي على رقم.");
    }
}
