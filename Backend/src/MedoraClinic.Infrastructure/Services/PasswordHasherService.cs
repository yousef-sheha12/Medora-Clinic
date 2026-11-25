using MedoraClinic.Application.Interfaces.IServices;
using MedoraClinic.Domain.Entities;
using Microsoft.AspNetCore.Identity;
namespace MedoraClinic.Infrastructure.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<User> hasher = new();

        public string Hash(User user, string password) =>
             hasher.HashPassword(user, password);


        public bool Verify(User user, string passwordHash, string password)
        {
            var result = hasher.VerifyHashedPassword(user, passwordHash, password);

            return result is PasswordVerificationResult.Success
                or PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
