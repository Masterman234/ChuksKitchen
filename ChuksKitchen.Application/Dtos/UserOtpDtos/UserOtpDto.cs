namespace ChuksKitchen.Application.Dtos.UserOtpDtos;

public record UserOtpDto(Guid Id, Guid UserId,DateTime ExpiresAt, DateTime CreatedAt, bool IsUsed);
public record CreateUserOtpDto(Guid UserId, string? Destination);
public record VerifyOtpRequest(string EmailOrPhone, string OtpCode);

