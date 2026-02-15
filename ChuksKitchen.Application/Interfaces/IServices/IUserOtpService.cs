namespace ChuksKitchen.Application.Interfaces.IServices;

public interface IUserOtpService
{
    Task GenerateOtpAsync(Guid userId);
    Task<bool> VerifyOtpAsync(Guid userId, string code);
}