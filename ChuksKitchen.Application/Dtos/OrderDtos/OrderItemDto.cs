namespace ChuksKitchen.Application.Dtos.OrderDtos;

public record OrderItemDto(Guid FoodItemId, string Name, decimal PriceAtOrder, int Quantity);

