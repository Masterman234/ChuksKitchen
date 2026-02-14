using ChuksKitchen.Domain.Entities;

public class CartItem : BaseEntity
{
    public Guid CartId { get; set; }

    public Guid FoodItemId { get; set; }

    public int Quantity { get; set; }

    // Navigation
    public Cart Cart { get; set; } = default!;
    public FoodItem FoodItem { get; set; } = default!;
}