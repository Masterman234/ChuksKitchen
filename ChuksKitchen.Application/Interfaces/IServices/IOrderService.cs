using ChuksKitchen.Application.Dtos.OrderDtos;
using ChuksKitchen.Domain.Enum;

namespace ChuksKitchen.Application.Interfaces.IServices;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(Guid userId);
    Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(Guid userId);
    Task<OrderDto?> GetOrderByIdAsync(Guid orderId);
    Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
}