using ChuksKitchen.Application.Dtos.FoodItemDtos;

namespace ChuksKitchen.Application.Interfaces.IServices;

public interface IFoodItemService
{
    Task<FoodItemDto> AddFoodItemAsync(FoodItemCreateDto dto, Guid creatorId);
    Task<IEnumerable<FoodItemDto>> GetAllFoodItemsAsync();
    Task<FoodItemDto?> GetFoodItemByIdAsync(Guid id);
    Task UpdateFoodItemAsync(Guid id, FoodItemUpdateDto dto);
    Task DeleteFoodItemAsync(Guid id);
}