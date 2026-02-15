using System.Diagnostics;
using System.Xml;
using ChuksKitchen.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChuksKitchen.Infrastructure.Persistence.Configurations;

internal class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Quantity).IsRequired();

        // Composite Unique Constraint: A cart cannot have the same food item twice
        builder.HasIndex(ci => new { ci.CartId, ci.FoodItemId }).IsUnique();


        builder.HasOne(ci => ci.Cart)
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ci => ci.FoodItem)
            .WithMany(fi => fi.CartItems)
            .HasForeignKey(ci => ci.FoodItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
