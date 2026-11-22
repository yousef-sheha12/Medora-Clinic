using MedoraClinic.Domain.Common;
using MedoraClinic.Domain.Enums;

namespace MedoraClinic.Domain.Entities
{
    public class Payment: BaseEntity
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        //public string TransactionReference { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; }


        public int PatientId { get; set; }
        public PatientProfile? PatientProfile { get; set; }
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
    }
}
