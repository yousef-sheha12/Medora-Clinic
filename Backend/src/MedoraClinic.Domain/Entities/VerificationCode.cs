using MedoraClinic.Domain.Common;
using MedoraClinic.Domain.Enums;

namespace MedoraClinic.Domain.Entities
{
    public class VerificationCode : BaseEntity
    {
        public VerificationType Type { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime? UsedAt { get; set; }


        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
