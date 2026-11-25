using MedoraClinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedoraClinic.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder.Property(user => user.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.Property(user => user.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(user => user.Role)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(user => user.IsEmailVerified)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(user => user.IsBlocked)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(user => user.DoctorProfile)
            .WithOne(profile => profile.User)
            .HasForeignKey<DoctorProfile>(profile => profile.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(user => user.PatientProfile)
            .WithOne(profile => profile.User)
            .HasForeignKey<PatientProfile>(profile => profile.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(user => user.VerificationCodes)
            .WithOne(code => code.User)
            .HasForeignKey(code => code.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(user => user.RefreshTokens)
            .WithOne(token => token.User)
            .HasForeignKey(token => token.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(user => user.Notifications)
            .WithOne(notification => notification.User)
            .HasForeignKey(notification => notification.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}