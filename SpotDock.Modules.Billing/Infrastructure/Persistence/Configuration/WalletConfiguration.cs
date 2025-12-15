using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotDock.Modules.Billing.Domain.Entities;

namespace SpotDock.Modules.Billing.Infrastructure.Persistence.Configuration;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("wallets");

        builder.HasKey(w => w.Id);

        builder.HasIndex(w => w.UserId)
            .IsUnique();

        builder.Property(w => w.Balance)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(w => w.CreatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(w => w.UpdatedAt)
            .HasColumnType("timestamp with time zone");
    }
}
