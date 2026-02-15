using ChuksKitchen.Application.Dtos.CartDtos;

namespace ChuksKitchen.Application.Interfaces.IServices;

public interface ICartService
{
    Task<CartDto> GetCartAsync(Guid userId);

    Task AddItemAsync(Guid userId, Guid foodItemId, int quantity);

    Task UpdateItemQuantityAsync(Guid userId, Guid foodItemId, int quantity);

    Task RemoveItemAsync(Guid userId, Guid foodItemId);

    Task ClearCartAsync(Guid userId);
}