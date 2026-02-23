using System.ComponentModel.DataAnnotations;
using ChuksKitchen.Domain.Enum;

namespace ChuksKitchen.Application.Dtos.UserDtos;

public record UserDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? Email,
    string? Phone,
    string ReferralCode,
    Guid? ReferredByUserId,
    UserRole Role
);

public record UserCreateDto(
    [Required]
    [MaxLength(100)]
    string FirstName,

    [Required]
    [MaxLength(100)]
    string LastName,

    [EmailAddress]
    [MaxLength(320)]
    string? Email,

    [Phone]
    [MaxLength(32)]
    string? Phone,

    [Required]
    UserRole Role,

    [MaxLength(10)]
    string? ReferralCode
    );

public record UserUpdateDto(

    [Required]
    [MaxLength(100)]
    string FirstName,

    [Required]
    [MaxLength(100)]
    string LastName,

    [EmailAddress]
    [MaxLength(320)]
    string? Email,

    [Phone]
    [MaxLength(32)]
    string? Phone,

    [Required]
    UserRole Role
);
