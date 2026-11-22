using MedoraClinic.Domain.Common;
using MedoraClinic.Domain.Enums;

namespace MedoraClinic.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string Passsword { get; set; } = string.Empty;
        public Role Role { get; set; }
        public bool IsBlocked { get; set; }

        //public string FirstName { get; set; } = string.Empty;
        //public string LastName { get; set; } = string.Empty;
        //public string PasswordHash { get; set; } = string.Empty;
        //public bool EmailConfirmed { get; set; }

        public DoctorProfile? DoctorProfile { get; set; }
        public PatientProfile? PatientProfile { get; set; }
        public ICollection<VerificationCode> VerificationCodes { get; set; } = new List<VerificationCode>();
        //public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
