using Bcs.Domain.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bcs.DataAccess.Configuration;

public class ClubConfiguration : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.ToTable("Clubs");
        builder.HasKey(c => c.Tag);
        builder.HasIndex(c => c.Tag)
            .IsUnique()
            .HasDatabaseName("IX_Clubs_Tag");

        builder.Property(c => c.Tag).IsRequired().HasMaxLength(20);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        
        builder.HasOne(c => c.LatestStats)
            .WithOne()
            .HasForeignKey<Club>(c => c.LatestStatsId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(c => c.ClubStats)
            .WithOne(cs => cs.Club)
            .HasForeignKey(cs => cs.ClubTag)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(c => c.Players)
            .WithOne(cs => cs.Club)
            .HasForeignKey(cs => cs.ClubTag)
            .OnDelete(DeleteBehavior.Restrict);
    }
    
}