using System.Reflection.Emit;
using ChuksKitchen.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChuksKitchen.Infrastructure.Persistence.Configurations;

internal class FoodItemConfiguration : IEntityTypeConfiguration<FoodItem>
{
    public void Configure(EntityTypeBuilder<FoodItem> builder)
    {
        builder.HasKey(fi => fi.Id);

        builder.Property(fi => fi.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(fi => fi.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(fi => fi.Description)
            .HasMaxLength(2000);

        builder.Property(fi => fi.ImageUrl)
              .HasMaxLength(500);

        builder.Property(fi => fi.IsAvailable).HasDefaultValue(true);

        // Add index on Name
        builder.HasIndex(fi => fi.Name);

        builder.HasOne(fi => fi.Creator)
            .WithMany()
            .HasForeignKey(fi => fi.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(fi => fi.CartItems)
            .WithOne(ci => ci.FoodItem)
            .HasForeignKey(ci => ci.FoodItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(fi => fi.OrderItems)
            .WithOne(oi => oi.FoodItem)
            .HasForeignKey(oi => oi.FoodItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
