using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.CartDtos;
using ChuksKitchen.Application.Interfaces.IRepositories;
using ChuksKitchen.Application.Interfaces.IServices;
using ChuksKitchen.Domain.Entities;

namespace ChuksKitchen.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IFoodItemRepository _foodItemRepository;

    public CartService(ICartRepository cartRepository, IFoodItemRepository foodItemRepository)
    {
        _cartRepository = cartRepository;
        _foodItemRepository = foodItemRepository;
    }

    public async Task<BaseResponseModel<CartDto>> AddItemAsync(Guid userId, Guid foodItemId, int quantity = 1)
    {
        try
        {
            if (quantity <= 0)
            {
                return BaseResponseModel<CartDto>.FailureResponse("Quantity must be at least 1.");
            }

            var foodItem = await _foodItemRepository.GetByIdAsync(foodItemId);
            if (foodItem == null || !foodItem.IsAvailable)
            {
                return BaseResponseModel<CartDto>.FailureResponse("Food item not found or unavailable.");
            }

            var cart = await _cartRepository.GetByIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepository.AddAsync(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.FoodItemId == foodItemId);
            if (existingItem != null)
            {
                // Increment quantity
                existingItem.Quantity += quantity;
            }

            else
            {
                // Add new item
                cart.Items.Add(new CartItem
                {
                    FoodItemId = foodItemId,
                    Quantity = quantity
                });


            }

            await _cartRepository.Update(cart);

            return BaseResponseModel<CartDto>.SuccessResponse(MapCartToDto(cart), "Item added to cart successfully.");
        }
        catch (Exception)
        {
            return BaseResponseModel<CartDto>.FailureResponse("An error occurred while adding item to cart.");
        }
    }

    // View Cart
    public async Task<BaseResponseModel<CartDto>> GetCartAsync(Guid userId)
    {
        try
        {
            var cart = await _cartRepository.GetCartWithItemsAsync(userId);
            if (cart == null)
            {
                return BaseResponseModel<CartDto>.SuccessResponse(new CartDto(Guid.Empty, userId, new List<CartItemDto>()), "Cart is empty.");
            }

            return BaseResponseModel<CartDto>.SuccessResponse(MapCartToDto(cart), "Cart retrieved successfully.");
        }
        catch (Exception)
        {

            return BaseResponseModel<CartDto>.FailureResponse("An error occurred while retrieving the cart.");
        }
    }


    // Clear Cart
    public async Task<BaseResponseModel<bool>> ClearCartAsync(Guid userId)
    {
        try
        {
            var cart = await _cartRepository.GetCartWithItemsAsync(userId);
            if (cart == null)
            {
                return BaseResponseModel<bool>.SuccessResponse(true, "Cart is already empty.");
            }

            cart.Items.Clear();
            await _cartRepository.Update(cart);

            return BaseResponseModel<bool>.SuccessResponse(true, "Cart cleared successfully.");

        }
        catch (Exception)
        {

            return BaseResponseModel<bool>.FailureResponse("An error occurred while clearing the cart.");
        }
    }

    // Remove a single item from the cart
    public async Task<BaseResponseModel<bool>> RemoveItemAsync(Guid userId, Guid foodItemId)
    {
        try
        {
            var cart = await _cartRepository.GetCartWithItemsAsync(userId);
            if (cart == null || !cart.Items.Any())
            {
                return BaseResponseModel<bool>.FailureResponse("Cart is empty.");
            }

            var item = cart.Items.FirstOrDefault(i => i.FoodItemId == foodItemId);
            if (item == null)
            {
                return BaseResponseModel<bool>.FailureResponse("Item not found in cart.");
            }

            cart.Items.Remove(item);
            await _cartRepository.Update(cart);

            return BaseResponseModel<bool>.SuccessResponse(true, "Item removed from cart.");
        }
        catch (Exception)
        {
            return BaseResponseModel<bool>.FailureResponse("An error occurred while removing the item from the cart.");
        }
    }

    // Update the quantity of a cart item
    public async Task<BaseResponseModel<CartDto>> UpdateItemQuantityAsync(Guid userId, Guid foodItemId, int quantity = 1)
    {
        try
        {
            if (quantity <= 0)
            {
                return BaseResponseModel<CartDto>.FailureResponse("Quantity must be at least 1.");
            }

            var cart = await _cartRepository.GetCartWithItemsAsync(userId);
            if (cart == null || !cart.Items.Any())
            {
                return BaseResponseModel<CartDto>.FailureResponse("Cart is empty.");
            }

            var item = cart.Items.FirstOrDefault(i => i.FoodItemId == foodItemId);
            if (item == null)
            {
                return BaseResponseModel<CartDto>.FailureResponse("Item not found in cart.");
            }

            item.Quantity = quantity;
            await _cartRepository.Update(cart);

            return BaseResponseModel<CartDto>.SuccessResponse(MapCartToDto(cart), "Item quantity updated successfully.");
        }
        catch (Exception)
        {
            return BaseResponseModel<CartDto>.FailureResponse("An error occurred while updating the item quantity.");
        }
    }


    // Helper: Map Cart to CartDto
    private CartDto MapCartToDto(Cart cart)
    {
        var items = cart.Items.Select(i => new CartItemDto(
            i.FoodItemId,
            i.FoodItem.Name,
            i.FoodItem.Price,
            i.Quantity
        )).ToList();

        return new CartDto(cart.Id, cart.UserId, items);
    }
}
