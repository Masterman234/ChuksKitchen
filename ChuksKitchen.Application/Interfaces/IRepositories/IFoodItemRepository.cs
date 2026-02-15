using ChuksKitchen.Domain.Entities;

namespace ChuksKitchen.Application.Interfaces.IRepositories;

public interface IFoodItemRepository
{
    Task<FoodItem?> GetByIdAsync(Guid id);
    Task<List<FoodItem>> GetAllAsync();
    Task<List<FoodItem>> GetAvailableAsync();
    Task AddAsync(FoodItem foodItem);
    void Update(FoodItem foodItem);
    void Remove(FoodItem foodItem);
    Task SaveChangesAsync();
}
