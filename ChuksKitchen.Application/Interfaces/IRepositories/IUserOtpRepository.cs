namespace ChuksKitchen.Application.Interfaces.IRepositories;

public interface IUserOtpRepository
{
    Task<UserOtp?> GetValidOtpAsync(Guid userId, string code);
    Task AddAsync(UserOtp otp);
    Task Update(UserOtp otp);
    Task InvalidatePreviousOtpsAsync(Guid userId);
 
}