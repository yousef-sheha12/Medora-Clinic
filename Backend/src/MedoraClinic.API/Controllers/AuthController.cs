using MedoraClinic.API.Extensions;
using MedoraClinic.Application.DTOs.Auth;
using MedoraClinic.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedoraClinic.API.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService service;
        public AuthController(IAuthService service) => this.service = service;


        [HttpPost("register/patient")]
        public async Task<IActionResult> RegisterPatient(
            [FromBody] RegisterPatientDto dto,
            CancellationToken cancellationToken)
        {
            var response = await service.RegisterPatientAsync(dto, cancellationToken);
            return this.ToActionResult(response);
        }

        [HttpPost("register/doctor")]
        public async Task<IActionResult> RegisterDoctor(
            [FromBody] RegisterDoctorDto dto,
            CancellationToken cancellationToken)
        {
            var response = await service.RegisterDoctorAsync(dto, cancellationToken);
            return this.ToActionResult(response);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(
            [FromBody] VerifyEmailRequest request,
            CancellationToken cancellationToken)
        {
            var response = await service.VerifyEmailAsync(request, cancellationToken);
            return this.ToActionResult(response);
        }

        [HttpPost("login")]
        
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken)
        {
            var response = await service.LoginAsync(request, cancellationToken);
            return this.ToActionResult(response);
        }

        [HttpPost("refresh")]
        
        public async Task<IActionResult> Refresh(
            [FromBody] RefreshTokenRequest request,
            CancellationToken cancellationToken)
        {
            var response = await service.RefreshAsync(request, cancellationToken);
            return this.ToActionResult(response);
        }

        [HttpPost("logout")]
        
        public async Task<IActionResult> Logout(
            [FromBody] RefreshTokenRequest request,
            CancellationToken cancellationToken)
        {
            var response = await service.LogoutAsync(request, cancellationToken);
            return this.ToActionResult(response);
        }

        [HttpPost("forgot-password")]
        
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordRequest request,
            CancellationToken cancellationToken)
        {
            var response = await service.ForgotPasswordAsync(request, cancellationToken);
            return this.ToActionResult(response);
        }

        [HttpPost("reset-password")]
        
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordRequest request,
            CancellationToken cancellationToken)
        {
            var response = await service.ResetPasswordAsync(request, cancellationToken);
            return this.ToActionResult(response);
        }
        
    }
}
