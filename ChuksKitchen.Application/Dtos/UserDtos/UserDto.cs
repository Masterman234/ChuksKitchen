using ChuksKitchen.Domain.Enum;

namespace ChuksKitchen.Application.Dtos.UserDtos;

public record UserDto( Guid Id, string Name, string Email, UserRole Role);

public record UserCreateDto(string Email, string? Phone, string Password, UserRole Role);

public record UserUpdateDto(string Name, string Email, UserRole Role);
