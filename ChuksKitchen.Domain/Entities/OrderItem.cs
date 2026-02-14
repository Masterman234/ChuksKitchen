using ChuksKitchen.Domain.Common;

namespace ChuksKitchen.Domain.Entities;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }

    public Guid FoodItemId { get; set; }

    public decimal PriceAtOrder { get; set; }

    public int Quantity { get; set; }

    // Navigation
    public Order Order { get; set; } = default!;
    public FoodItem FoodItem { get; set; } = default!;
}