using MedoraClinic.Domain.Entities;

namespace MedoraClinic.Application.Interfaces.IServices
{
    public interface IPasswordHasherService
    {
        string Hash(User user, string password);
        bool Verify(User user, string passwordHash, string password);
    }
}
