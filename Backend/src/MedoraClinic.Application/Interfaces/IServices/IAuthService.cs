using MedoraClinic.Application.Common;
using MedoraClinic.Application.DTOs.Auth;

namespace MedoraClinic.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> RegisterPatientAsync(RegisterPatientDto dto, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> RegisterDoctorAsync(RegisterDoctorDto dto, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> VerifyEmailAsync(VerifyEmailRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<TokenResponse>> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> LogoutAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<object>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);
        
    }
}
