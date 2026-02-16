using ChuksKitchen.Application.Interfaces.IServices;

namespace ChuksKitchen.Infrastructure.Services;

public class FakeEmailService : IEmailService
{
    public Task SendOtpAsync(string toEmail, string otpCode)
    {
        Console.WriteLine("====================================");
        Console.WriteLine($"Sending OTP to: {toEmail}");
        Console.WriteLine($"OTP Code: {otpCode}");
        Console.WriteLine("====================================");

        return Task.CompletedTask;
    }
}
