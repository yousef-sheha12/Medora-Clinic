using FluentValidation;
using MedoraClinic.Application.DTOs.Auth;

namespace MedoraClinic.Application.Validators.Auth;

public sealed class RegisterDoctorDtoValidator
    : AbstractValidator<RegisterDoctorDto>
{
    public RegisterDoctorDtoValidator()
    {
        RuleFor(request => request.FullName)
            .NotEmpty().WithMessage("الاسم مطلوب.")
            .MaximumLength(150).WithMessage("الاسم يجب ألا يتجاوز 150 حرفًا.");

        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("البريد الإلكتروني مطلوب.")
            .EmailAddress().WithMessage("صيغة البريد الإلكتروني غير صحيحة.")
            .MaximumLength(255).WithMessage("البريد الإلكتروني طويل جدًا.");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("كلمة المرور مطلوبة.")
            .MinimumLength(10).WithMessage("كلمة المرور يجب أن تكون 10 أحرف على الأقل.")
            .Matches("[A-Z]").WithMessage("كلمة المرور يجب أن تحتوي على حرف كبير.")
            .Matches("[a-z]").WithMessage("كلمة المرور يجب أن تحتوي على حرف صغير.")
            .Matches("[0-9]").WithMessage("كلمة المرور يجب أن تحتوي على رقم.");

        RuleFor(request => request.ContactNumber)
            .NotEmpty().WithMessage("رقم التواصل مطلوب.")
            .Matches(@"^\+?[0-9 ()-]{7,20}$")
            .WithMessage("رقم التواصل غير صالح.");

        RuleFor(request => request.Bio)
            .NotEmpty().WithMessage("النبذة التعريفية مطلوبة.")
            .MaximumLength(4000).WithMessage("النبذة التعريفية طويلة جدًا.");

        RuleFor(request => request.Price)
            .GreaterThan(0).WithMessage("السعر يجب أن يكون أكبر من صفر.");

        RuleFor(request => request.SpecialityId)
            .GreaterThan(0).WithMessage("يجب اختيار تخصص صحيح.");
    }
}
