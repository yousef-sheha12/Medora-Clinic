using MedoraClinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedoraClinic.Infrastructure.Data.Configurations;

public class VerificationCodeConfiguration
    : IEntityTypeConfiguration<VerificationCode>
{
    public void Configure(EntityTypeBuilder<VerificationCode> builder)
    {
        builder.HasKey(code => code.Id);

        builder.Property(code => code.TokenHash)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(code => code.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(code => code.ExpiresAt)
            .IsRequired();

        builder.HasIndex(code => new { code.UserId, code.Type });

        builder.HasOne(code => code.User)
            .WithMany(user => user.VerificationCodes)
            .HasForeignKey(code => code.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}