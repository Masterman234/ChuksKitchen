using ChuksKitchen.Domain.Common;
using ChuksKitchen.Domain.Entities;
public class UserOtp : BaseEntity
{
    public Guid UserId { get; set; }

    public string Code { get; set; } = default!;

    public bool IsUsed { get; set; }

    public DateTime ExpiresAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
}

