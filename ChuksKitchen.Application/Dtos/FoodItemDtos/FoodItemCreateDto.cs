namespace ChuksKitchen.Application.Dtos.FoodItemDtos;

public record FoodItemCreateDto(string Name, decimal Price, string? Description, bool IsAvailable = true);
