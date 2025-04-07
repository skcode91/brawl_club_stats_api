using Bcs.Domain.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bcs.DataAccess.Configuration;

public class ClubStatsConfiguration : IEntityTypeConfiguration<ClubStats>
{
    public void Configure(EntityTypeBuilder<ClubStats> builder)
    {
        builder.ToTable("ClubStats");
        builder.HasKey(cs => cs.Id);
        
        builder.Property(cs => cs.Trophies).IsRequired();
        builder.Property(cs => cs.CreatedAt).IsRequired();
        
        builder.HasOne(cs => cs.Club)
            .WithMany(c => c.ClubStats)
            .HasForeignKey(cs => cs.ClubId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}