using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ChuksKitchen.Application.Dtos.FoodItemDtos;

public record FoodItemUpdateDto(
     [Required]
    [MaxLength(100)]
    string Name,

    [Required]
    decimal Price,

    [MaxLength(500)]
    string? Description,

    IFormFile Image,
     [Required]
    bool IsAvailable = true);
