using ChuksKitchen.Domain.Common;
using ChuksKitchen.Domain.Enum;
namespace ChuksKitchen.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; }
    public string? Email { get; set; } = default!;
    public string? Phone { get; set; }

    public UserRole Role { get; set; }

    public bool IsVerified { get; set; }

    // Navigation
    public ICollection<UserOtp> Otps { get; set; } = new List<UserOtp>();
    public Cart? Cart { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
