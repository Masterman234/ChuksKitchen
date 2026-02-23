using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.UserOtpDtos;

namespace ChuksKitchen.Application.Interfaces.IServices;

public interface IUserOtpService
{
    Task<BaseResponseModel<UserOtpDto>> GenerateOtpAsync(CreateUserOtpDto request);

    Task<BaseResponseModel<string>> ValidateOtpAsync(Guid userId, string code);
}
