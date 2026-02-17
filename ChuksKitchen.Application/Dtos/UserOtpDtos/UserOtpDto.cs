namespace ChuksKitchen.Application.Dtos.UserOtpDtos;

public record UserOtpDto(Guid Id, Guid UserId,DateTime ExpiresAt, DateTime CreatedAt, bool IsUsed);
public record CreateUserOtpDto(Guid UserId);
public record VerifyOtpRequest(string EmailOrPhone, string OtpCode);

