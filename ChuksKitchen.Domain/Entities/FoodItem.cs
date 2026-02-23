using ChuksKitchen.Domain.Common;

namespace ChuksKitchen.Domain.Entities;

public class FoodItem : BaseEntity
{
    public string Name { get; set; } = default!;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsAvailable { get; set; } = true;

    public Guid CreatedBy { get; set; }

    // Navigation
    public User Creator { get; set; } = default!;
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}

