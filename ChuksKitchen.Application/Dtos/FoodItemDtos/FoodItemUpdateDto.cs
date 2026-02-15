namespace ChuksKitchen.Application.Dtos.FoodItemDtos;

public record FoodItemUpdateDto(string Name, decimal Price, string? Description, bool IsAvailable);