using MedoraClinic.Domain.Common;
using MedoraClinic.Domain.Enums;

namespace MedoraClinic.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public string Notes { get; set; } = string.Empty;


        public int PatientId { get; set; }
        public PatientProfile? PatientProfile { get; set; }
        public int DoctorId { get; set; }
        public DoctorProfile? DoctorProfile { get; set; }
        public int AvailabilityId { get; set; }
        public Availability? Availability { get; set; }


        public Payment? Payment { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
