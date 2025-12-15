using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotDock.Modules.Billing.Domain.Entities;

namespace SpotDock.Modules.Billing.Infrastructure.Persistence.Configuration;

public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
{
    public void Configure(EntityTypeBuilder<WalletTransaction> builder)
    {
        builder.ToTable("wallet_transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Amount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(t => t.Description)
            .HasMaxLength(512);

        builder.HasIndex(t => t.WalletId);
    }
}
