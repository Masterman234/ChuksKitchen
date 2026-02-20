using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.CartDtos;

namespace ChuksKitchen.Application.Interfaces.IServices;

public interface ICartService
{
    Task<BaseResponseModel<CartDto>> GetCartAsync(Guid userId);

    Task<BaseResponseModel<CartDto>> AddItemAsync(Guid userId, Guid foodItemId, int quantity = 1);

    Task<BaseResponseModel<CartDto>> UpdateItemQuantityAsync(Guid userId, Guid foodItemId, int quantity = 1);

    Task<BaseResponseModel<bool>> RemoveItemAsync(Guid userId, Guid foodItemId);

    Task<BaseResponseModel<bool>> ClearCartAsync(Guid userId);
}