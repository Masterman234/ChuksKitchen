using ChuksKitchen.Domain.Common;
using ChuksKitchen.Domain.Enum;
namespace ChuksKitchen.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; } = default!;
    public string? Phone { get; set; } = default!;

    public UserRole Role { get; set; }

    public bool IsVerified { get; set; }

    public string ReferralCode { get; set; } = default!;     // Each user gets a unique code
    public Guid? ReferredByUserId { get; set; }
    public User? ReferredByUser { get; set; } 

    // Navigation
    public ICollection<UserOtp> Otps { get; set; } = new List<UserOtp>();
    public Cart? Cart { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<User> ReferredUsers { get; set; } = new List<User>(); // Users this user referred
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
