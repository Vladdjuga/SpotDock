using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotDock.Modules.Auth.Domain.Entities;

namespace SpotDock.Modules.Auth.Infrastructure.Persistence.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.DisplayName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.CreatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(u => u.LastLoginAt)
            .HasColumnType("timestamp with time zone");
    }
}
