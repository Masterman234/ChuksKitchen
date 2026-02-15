namespace ChuksKitchen.Application.Dtos.UserDtos;

public record UserDto( Guid Id, string Name, string Email, string Role);

public record UserCreateDto(string Email, string? Phone, string Password, string Role);