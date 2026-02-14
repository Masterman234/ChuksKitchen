using ChuksKitchen.Domain.Common;
using ChuksKitchen.Domain.Enum;

namespace ChuksKitchen.Domain.Entities;

public class Order : BaseEntity
{
    public Guid UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    // Navigation
    public User User { get; set; } = default!;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}