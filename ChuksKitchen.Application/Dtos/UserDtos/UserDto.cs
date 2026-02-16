using ChuksKitchen.Domain.Enum;

namespace ChuksKitchen.Application.Dtos.UserDtos;

public record UserDto( Guid Id, string FullName, string Email, UserRole Role);

public record UserCreateDto(string FullName, string Email, string? Phone, UserRole Role);

public record UserUpdateDto(string FullName, string Email, UserRole Role);
