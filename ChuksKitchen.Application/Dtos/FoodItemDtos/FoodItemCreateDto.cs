using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ChuksKitchen.Application.Dtos.FoodItemDtos;

public record FoodItemCreateDto(
    [Required]
    [MaxLength(100)]
    string Name,

    [Required]
    decimal Price,

    [MaxLength(500)]
    string? Description,

     [Required]
    IFormFile Image,
     [Required]
    bool IsAvailable = true);
