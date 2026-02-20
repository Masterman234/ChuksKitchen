using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.OrderDtos;
using ChuksKitchen.Application.Interfaces.IServices;
using ChuksKitchen.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace ChuksKitchen.API.Controllers;

[ApiController]
[Route("order[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    // Place order
    [HttpPost("place/{userId}")]
    [ProducesResponseType(typeof(BaseResponseModel<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponseModel<OrderDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PlaceOrder(Guid userId)
    {
        var response = await _orderService.PlaceOrderAsync(userId);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
    // Get order by Id
    [HttpGet("{orderId}")]
    [ProducesResponseType(typeof(BaseResponseModel<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponseModel<OrderDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderById(Guid orderId)
    {
        var response = await _orderService.GetOrderByIdAsync(orderId);


        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
    // Get User orders
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(BaseResponseModel<List<OrderDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserOrders(Guid userId)
    {
        var response = await _orderService.GetUserOrdersAsync(userId);

        return Ok(response);
    }
    // Update order status
    [HttpPut("{orderId}/status")]
    [ProducesResponseType(typeof(BaseResponseModel<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponseModel<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponseModel<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromQuery] OrderStatus status)
    {
        var response = await _orderService.UpdateOrderStatusAsync(orderId, status);

        if (!response.Success)
        {
            if (response.Message == "Order not found")
            {
                return NotFound(response);
            }   

            return BadRequest(response);
        }

        return Ok(response);
    }
}
