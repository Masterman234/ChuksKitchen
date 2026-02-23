namespace ChuksKitchen.Application.Dtos.RatingDto;

public record CreateRatingRequest(Guid UserId, Guid FoodItemId, int Score, string? Comment);

public record RatingResponse(Guid Id,Guid UserId, Guid FoodItemId, int Score, string? Comment,DateTime CreatedAt);
