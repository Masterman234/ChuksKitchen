using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.UserOtpDtos;
using ChuksKitchen.Application.Interfaces.IIdentity;
using ChuksKitchen.Application.Interfaces.IRepositories;
using ChuksKitchen.Application.Interfaces.IServices;
using ChuksKitchen.Domain.Entities;

namespace ChuksKitchen.Application.Services;

public class UserOtpService : IUserOtpService
{
    private readonly IUserOtpRepository _userOtpRepository;
    private readonly IOtpGenerator _otpGenerator;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;

    public UserOtpService(IUserOtpRepository userOtpRepository, IOtpGenerator otpGenerator, IEmailService emailService, IUserRepository userRepository)
    {
        _userOtpRepository = userOtpRepository;
        _otpGenerator = otpGenerator;
        _emailService = emailService;
        _userRepository = userRepository;
    }

    public async Task<BaseResponseModel<UserOtpDto>> GenerateOtpAsync(CreateUserOtpDto request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return BaseResponseModel<UserOtpDto>.FailureResponse("User not found");
            }

            await _userOtpRepository.InvalidatePreviousOtpsAsync(request.UserId);
            var code = _otpGenerator.GenerateOtp();

            var userOtp = new UserOtp
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Code = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                CreatedAt = DateTime.UtcNow,
                IsUsed = false
            };
            await _userOtpRepository.AddAsync(userOtp);
            await _emailService.SendOtpAsync(user.Email, code);

            var userOtpDto = new UserOtpDto(
               userOtp.Id,
               userOtp.UserId,
               userOtp.ExpiresAt,
               userOtp.CreatedAt,
               userOtp.IsUsed

            );




            return BaseResponseModel<UserOtpDto>.SuccessResponse(userOtpDto, "OTP generated successfully");
        }
        catch (Exception)
        {
            return BaseResponseModel<UserOtpDto>.FailureResponse("Failed to generate OTP");
        }
    }

    async Task<BaseResponseModel<string>> IUserOtpService.ValidateOtpAsync(Guid userId, string code)
    {
        try
        {
            var otp = await _userOtpRepository.GetValidOtpAsync(userId, code);
            if (otp == null)
            {
                return BaseResponseModel<string>.FailureResponse("Invalid or expired OTP");
            }
            otp.IsUsed = true;
            await _userOtpRepository.Update(otp);
            return BaseResponseModel<string>.SuccessResponse("OTP validated successfully");
        }
        catch (Exception)
        {

            return BaseResponseModel<string>.FailureResponse("Invalid or expired OTP");
        }
    }
}
