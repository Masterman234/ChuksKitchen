namespace ChuksKitchen.Application.Interfaces.IServices;

public interface IEmailService
{
    Task SendOtpAsync(string toEmail, string otpCode);
}

