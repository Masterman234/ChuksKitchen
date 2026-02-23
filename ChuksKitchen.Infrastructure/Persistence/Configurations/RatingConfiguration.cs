using ChuksKitchen.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChuksKitchen.Infrastructure.Persistence.Configurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable("Ratings");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Score)
            .IsRequired();

        builder.Property(r => r.Comment)
            .HasMaxLength(500);

        builder.HasOne(r => r.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.FoodItem)
            .WithMany(f => f.Ratings)
            .HasForeignKey(r => r.FoodItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Prevent duplicate rating by same user
        builder.HasIndex(r => new { r.UserId, r.FoodItemId })
            .IsUnique();
    }
}
