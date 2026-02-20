namespace ChuksKitchen.Application.Dtos.OrderDtos;

public record OrderItemDto(Guid FoodItemId, string FoodItemName, decimal PriceAtOrder, int Quantity);

