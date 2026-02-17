using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.FoodItemDtos;

namespace ChuksKitchen.Application.Interfaces.IServices;

public interface IFoodItemService
{
    Task<BaseResponseModel<FoodItemDto>> AddFoodItemAsync(FoodItemCreateDto request, Guid adminUserId);
    Task<BaseResponseModel<IEnumerable<FoodItemDto>>> GetAllFoodItemsAsync();
    Task<BaseResponseModel<IEnumerable<FoodItemDto>>> GetAvailableAsync();
    Task<BaseResponseModel<FoodItemDto?>> GetFoodItemByIdAsync(Guid foodId);
    Task<BaseResponseModel<FoodItemDto>>UpdateFoodItemAsync(Guid foodId, FoodItemUpdateDto dto, Guid adminUserId);
    Task<BaseResponseModel<bool>>DeleteFoodItemAsync(Guid foodId, Guid adminUserId);
}