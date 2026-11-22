using MedoraClinic.Domain.Common;

namespace MedoraClinic.Domain.Entities
{
    public class DoctorLocation : BaseEntity
    {
        public int DoctorId { get; set; }
        public int LocationId { get; set; }

        public DoctorProfile? DoctorProfile { get; set; }
        public Location? Location { get; set; }
    }
}
