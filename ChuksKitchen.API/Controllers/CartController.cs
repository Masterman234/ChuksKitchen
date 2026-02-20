using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.CartDtos;
using ChuksKitchen.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ChuksKitchen.API.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    // Get cart by id
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(BaseResponseModel<CartDto>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<CartDto>), 404)]
    public async Task<IActionResult> GetCart(Guid userId)
    {
        var result = await _cartService.GetCartAsync(userId);
        if (!result.Success)
        {
            return NotFound(result);
        }  
        return Ok(result);
    }

    // Add item to cart
    [HttpPost("add")]
    [ProducesResponseType(typeof(BaseResponseModel<CartDto>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<CartDto>), 400)]
    public async Task<IActionResult> AddItem([FromQuery] Guid userId, [FromQuery] Guid foodItemId, [FromQuery] int quantity = 1)
    {
        var result = await _cartService.AddItemAsync(userId, foodItemId, quantity);
        if (!result.Success)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    // Update cart
    [HttpPut("update")]
    [ProducesResponseType(typeof(BaseResponseModel<CartDto>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<CartDto>), 400)]
    public async Task<IActionResult> UpdateItemQuantity([FromQuery] Guid userId, [FromQuery] Guid foodItemId, [FromQuery] int quantity)
    {
        var result = await _cartService.UpdateItemQuantityAsync(userId, foodItemId, quantity);
        if (!result.Success)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    // Delete cart
    [HttpDelete("remove")]
    [ProducesResponseType(typeof(BaseResponseModel<bool>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<bool>), 400)]
    public async Task<IActionResult> RemoveItem([FromQuery] Guid userId, [FromQuery] Guid foodItemId)
    {
        var result = await _cartService.RemoveItemAsync(userId, foodItemId);
        if (!result.Success)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    // Clear cart
    [HttpDelete("clear")]
    [ProducesResponseType(typeof(BaseResponseModel<bool>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<bool>), 400)]
    public async Task<IActionResult> ClearCart([FromQuery] Guid userId)
    {
        var result = await _cartService.ClearCartAsync(userId);
        if (!result.Success)
        {
            return NotFound(result);
        }
        return Ok(result);
    }
}
