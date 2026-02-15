namespace ChuksKitchen.Application.Dtos.CartDtos;

public record CartItemDto(Guid FoodItemId, string Name, decimal Price, int Quantity);

