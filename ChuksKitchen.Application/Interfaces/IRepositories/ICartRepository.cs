using ChuksKitchen.Domain.Entities;

namespace ChuksKitchen.Application.Interfaces.IRepositories;

public interface ICartRepository
{
    Task<Cart?> GetByIdAsync(Guid id);
    Task<Cart?> GetCartWithItemsAsync(Guid userId);
    Task<Cart?> GetByUserIdAsync(Guid userId);
    Task<bool> ClearCartAsync(Cart cart);
    Task AddAsync(Cart cart);
    Task Update(Cart cart);
    Task Remove(Cart cart);
}
