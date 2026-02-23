using System.ComponentModel.DataAnnotations;

namespace ChuksKitchen.Application.Dtos.OrderDtos;

public record OrderItemDto(
    [Required]
    Guid FoodItemId,
    [Required]
    [MaxLength(200)]
    string FoodItemName,
    [Required]
    decimal PriceAtOrder,
    [Required]
    int Quantity);

