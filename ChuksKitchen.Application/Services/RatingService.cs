using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.RatingDto;
using ChuksKitchen.Application.Interfaces.IRepositories;
using ChuksKitchen.Application.Interfaces.IServices;
using ChuksKitchen.Domain.Entities;

namespace ChuksKitchen.Application.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFoodItemRepository _foodRepository;

    public RatingService(
        IRatingRepository ratingRepository,
        IUserRepository userRepository,
        IFoodItemRepository foodRepository)
    {
        _ratingRepository = ratingRepository;
        _userRepository = userRepository;
        _foodRepository = foodRepository;
    }

    public async Task<BaseResponseModel<RatingResponse>> CreateRatingAsync(Guid userId, Guid foodItemId, int score, string? comment)
    {
        try
        {
            // 1️⃣ Get user with orders
            var user = await _userRepository.GetByIdWithOrdersAsync(userId);
            if (user == null)
            {
                return BaseResponseModel<RatingResponse>.FailureResponse("User not found.");
            }

            // Check verified
            if (!user.IsVerified)
            {
                return BaseResponseModel<RatingResponse>.FailureResponse("User must be verified to rate.");
            }

            // Check food exists
            var foodItem = await _foodRepository.GetByIdAsync(foodItemId);
            if (foodItem == null)
            {
                return BaseResponseModel<RatingResponse>.FailureResponse("Food item not found.");
            }

            // Check user has ordered the food
            var hasOrdered = user.Orders
                .SelectMany(o => o.Items)
                .Any(oi => oi.FoodItemId == foodItemId);

            if (!hasOrdered)
            {
                return BaseResponseModel<RatingResponse>.FailureResponse("User can only rate food they have ordered.");
            }

            // Check duplicate rating
            var alreadyRated = await _ratingRepository.ExistsAsync(userId, foodItemId);
            if (alreadyRated)
            {
                return BaseResponseModel<RatingResponse>.FailureResponse("User has already rated this food item.");
            }

            // Create rating
            var rating = new Rating(userId, foodItemId, score, comment);

            await _ratingRepository.AddAsync(rating);

            var response = new RatingResponse(
                rating.Id,
                rating.UserId,
                rating.FoodItemId,
                rating.Score,
                rating.Comment,
                rating.CreatedAt
            );

            return BaseResponseModel<RatingResponse>.SuccessResponse(response, "Rating submitted successfully.");
        }


        catch (Exception ex)
        {

           return BaseResponseModel<RatingResponse>.FailureResponse($"An error occurred while submitting the rating: {ex.Message}");
        }
    }
}