using ChuksKitchen.Domain.Enum;

namespace ChuksKitchen.Application.Dtos.OrderDtos;

public record OrderDto(Guid Id, Guid UserId, decimal TotalPrice, OrderStatus Status, List<OrderItemDto> Items);

