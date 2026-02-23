using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.RatingDto;

namespace ChuksKitchen.Application.Interfaces.IServices;

public interface IRatingService
{
    Task<BaseResponseModel<RatingResponse>> CreateRatingAsync(Guid userId,Guid foodItemId,int score,string? comment);
}