using ChuksKitchen.Domain.Common;

namespace ChuksKitchen.Domain.Entities;

public class Cart : BaseEntity
{
    public Guid UserId { get; set; }

    // Navigation
    public User User { get; set; } = default!;
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
