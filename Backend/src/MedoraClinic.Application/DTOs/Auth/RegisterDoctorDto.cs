
namespace MedoraClinic.Application.DTOs.Auth
{
    public class RegisterDoctorDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int SpecialityId { get; set; }
    }
}
