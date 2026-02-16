using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.UserDtos;

namespace ChuksKitchen.Application.Interfaces.IServices;

public interface IUserService
{
    Task<BaseResponseModel<UserDto>> SignUpAsync(UserCreateDto request);
    Task<BaseResponseModel<bool>> VerifyOtpAsync(string emailOrPhone, string otpCode);
    Task<BaseResponseModel<UserDto?>> GetUserByIdAsync(Guid id);
    Task<BaseResponseModel<IEnumerable<UserDto>>> GetAllUsersAsync();
    Task<BaseResponseModel<Guid>> UpdateUserAsync(Guid id, UserUpdateDto request);
    Task<BaseResponseModel<bool>> DeleteUserAsync(Guid id);
}

