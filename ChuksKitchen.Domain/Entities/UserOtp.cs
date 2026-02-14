using ChuksKitchen.Domain.Entities;
public class UserOtp
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Code { get; set; } = default!;

    public bool IsUsed { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
}

