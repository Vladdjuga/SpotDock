using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpotDock.Modules.Auctions.Domain.Entities;

namespace SpotDock.Modules.Auctions.Infrastructure.Persistence.Configuration;

public class SpotInstanceConfiguration:IEntityTypeConfiguration<SpotInstance>
{
    public void Configure(EntityTypeBuilder<SpotInstance> builder)
    {
        builder.ToTable("spot_instances");

        builder.Property(b => b.EndsAt)
            .HasColumnType("timestamp with time zone");
    }
}