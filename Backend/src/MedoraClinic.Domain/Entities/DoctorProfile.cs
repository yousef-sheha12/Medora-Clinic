using MedoraClinic.Domain.Common;

namespace MedoraClinic.Domain.Entities
{
    public class DoctorProfile : BaseEntity
    {
        public decimal Price { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public bool IsApproved { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int SpecialityId { get; set; }
        public Speciality? Speciality { get; set; }

        public ICollection<DoctorLocation> DoctorLocations { get; set; } = new List<DoctorLocation>();
        public ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}
