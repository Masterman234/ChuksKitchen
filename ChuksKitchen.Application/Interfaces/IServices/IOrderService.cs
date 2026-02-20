using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.OrderDtos;
using ChuksKitchen.Domain.Enum;

namespace ChuksKitchen.Application.Interfaces.IServices;

public interface IOrderService
{
    Task<BaseResponseModel<OrderDto>> PlaceOrderAsync(Guid userId);
    Task<BaseResponseModel<OrderDto>> GetOrderByIdAsync(Guid orderId);
    Task<BaseResponseModel<List<OrderDto>>> GetUserOrdersAsync(Guid userId);
    Task<BaseResponseModel<bool>> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
}
