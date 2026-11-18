namespace MedoraClinic.Domain.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt = DateTime.UtcNow;
        public DateTime? UpdatedAt = DateTime.UtcNow;
    }
}
