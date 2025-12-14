using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotDock.Modules.Auctions.Domain.Entities;

namespace SpotDock.Modules.Auctions.Infrastructure.Persistence.Configuration;

public class BidConfiguration:IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder.ToTable("bids");

        builder.Property(b => b.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne<SpotInstance>()
            .WithMany(spot => spot.Bids)
            .OnDelete(DeleteBehavior.Cascade);
    }
}