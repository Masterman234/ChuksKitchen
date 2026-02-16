using System.Security.Cryptography;
using ChuksKitchen.Application.Interfaces.IIdentity;

namespace ChuksKitchen.Infrastructure.Identity;

public class OtpGenerator : IOtpGenerator
{
    public string GenerateOtp()
    {
        var bytes = RandomNumberGenerator.GetBytes(4);
        var number = BitConverter.ToUInt32(bytes, 0) % 900000 + 100000;
        return number.ToString();
    }
}
