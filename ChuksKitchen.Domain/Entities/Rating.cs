using ChuksKitchen.Domain.Common;

namespace ChuksKitchen.Domain.Entities;

public class Rating : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid FoodItemId { get; private set; }

    public int Score { get; private set; }  // 1 - 5
    public string? Comment { get; private set; }

    // Navigation
    public User User { get; private set; } = default!;
    public FoodItem FoodItem { get; private set; } = default!;

    private Rating() { } // For EF

    public Rating(Guid userId, Guid foodItemId, int score, string? comment)
    {
        if (score < 1 || score > 5)
            throw new ArgumentException("Rating score must be between 1 and 5.");

        UserId = userId;
        FoodItemId = foodItemId;
        Score = score;
        Comment = comment;
    }
}