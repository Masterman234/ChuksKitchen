using Microsoft.AspNetCore.Http;

namespace ChuksKitchen.Application.Dtos.FoodItemDtos;

public record FoodItemDto(Guid Id, string Name, decimal Price, string? Description, string? ImageUrl, bool IsAvailable);
