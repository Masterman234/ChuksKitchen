namespace ChuksKitchen.Application.Interfaces.IRepositories;

public interface IUserOtpRepository
{
    Task<UserOtp?> GetValidOtpAsync(Guid userId, string code);
    Task AddAsync(UserOtp otp);
    void Update(UserOtp otp);
    Task SaveChangesAsync();
}