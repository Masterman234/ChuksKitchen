using ChuksKitchen.Domain.Entities;

namespace ChuksKitchen.Application.Interfaces.IRepositories;

public interface IFoodItemRepository
{
    Task<FoodItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<FoodItem>> GetAllAsync();
    Task<IEnumerable<FoodItem>> GetAvailableAsync();
    Task AddAsync(FoodItem foodItem);
    Task UpdateAsync(FoodItem foodItem);
    Task DeleteAsync(FoodItem foodItem);
}
