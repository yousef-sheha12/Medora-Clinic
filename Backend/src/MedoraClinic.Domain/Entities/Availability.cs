using MedoraClinic.Domain.Common;

namespace MedoraClinic.Domain.Entities
{
    public class Availability : BaseEntity
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; }


        public int DoctorId { get; set; }
        public DoctorProfile? DoctorProfile { get; set; }
        public int LocationId { get; set; }
        public Location? Location { get; set; }


        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
