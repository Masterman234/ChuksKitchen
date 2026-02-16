using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.UserDtos;
using ChuksKitchen.Application.Dtos.UserOtpDtos;
using ChuksKitchen.Application.Interfaces.IIdentity;
using ChuksKitchen.Application.Interfaces.IRepositories;
using ChuksKitchen.Application.Interfaces.IServices;
using ChuksKitchen.Domain.Entities;

namespace ChuksKitchen.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IUserOtpService _userOtpService;

    public UserService(IUserRepository userRepository, ICartRepository cartRepository, IUserOtpService userOtpService)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _userOtpService = userOtpService;

    }

    public async Task<BaseResponseModel<UserDto>> SignUpAsync(UserCreateDto request)
    {
        try
        {
            if (request == null)
            {
                return (BaseResponseModel<UserDto>.FailureResponse("Invalid user data."));
            }

            //Find user by email or phone
            var existByEmail = await _userRepository.GetByEmailAsync(request.Email);
            if (existByEmail != null)
            {
                return BaseResponseModel<UserDto>.FailureResponse("Email is already registered.");
            }

            var existByPhone = await _userRepository.GetByPhone(request.Phone);
            if (existByPhone != null)
            {
                return BaseResponseModel<UserDto>.FailureResponse("Phone number is already registered.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                Role = request.Role,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            //Create Cart automatically for the user
            var cart = new Cart
            {
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
            };

            await _cartRepository.AddAsync(cart);

            //Generate OTP for email or phone verification via OtpService
            await _userOtpService.GenerateOtpAsync(new CreateUserOtpDto(user.Id));


            var dto = new UserDto(user.Id, user.FullName, user.Email, user.Role);

            return BaseResponseModel<UserDto>.SuccessResponse(dto, "User registered successfully.");


        }
        catch (Exception)
        {
            return BaseResponseModel<UserDto>.FailureResponse("An error occurred while registering the user.");
        }
    }


    public async Task<BaseResponseModel<bool>> VerifyOtpAsync(string emailOrPhone, string otpCode)
    {
        try
        {
            //Find user by email or phone
            var user = await _userRepository.GetByEmailAsync(emailOrPhone) ?? await _userRepository.GetByPhone(emailOrPhone);
            {
                if (user == null)
                {
                    return BaseResponseModel<bool>.FailureResponse("User not found.");
                }
            }
            var otpValidation = await _userOtpService.ValidateOtpAsync(user.Id, otpCode);
            if (!otpValidation.Success)
            {
                return BaseResponseModel<bool>.FailureResponse(otpValidation.Message);
            }

            //Mark user as verified
            user.IsVerified = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.Update(user);

            return (BaseResponseModel<bool>.SuccessResponse(true, "OTP verified successfully. Your account is now verified."));

        }
        catch (Exception)
        {

            return BaseResponseModel<bool>.FailureResponse("An error occurred while verifying the OTP.");
        }
    }

    public async Task<BaseResponseModel<UserDto?>> GetUserByIdAsync(Guid id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            {
                if (user == null)
                {
                    return BaseResponseModel<UserDto?>.FailureResponse("User not found.");
                }
            }
            var userDto = new UserDto(user.Id, user.FullName, user.Email, user.Role);
            return BaseResponseModel<UserDto?>.SuccessResponse(userDto, "User retrieved successfully.");

        }
        catch (Exception)
        {

            return BaseResponseModel<UserDto?>.FailureResponse("An error occurred while retrieving the user.");
        }
    }


    public async Task<BaseResponseModel<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userDtos = users.Select(u => new UserDto(u.Id, u.FullName, u.Email, u.Role));

            return BaseResponseModel<IEnumerable<UserDto>>.SuccessResponse(userDtos, "Users retrieved successfully.");
        }
        catch (Exception)
        {
            return BaseResponseModel<IEnumerable<UserDto>>.FailureResponse("An error occurred while retrieving users.");
        }
    }

    public async Task<BaseResponseModel<Guid>> UpdateUserAsync(Guid id, UserUpdateDto request)
    {
        try
        {
            if (request == null)
            {
                return BaseResponseModel<Guid>.FailureResponse("Invalid user data.");
            }

            var user = await _userRepository.GetByIdAsync(id);
            {
                if (user == null)
                {
                    return BaseResponseModel<Guid>.FailureResponse("User not found.");
                }
            }

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.Role = request.Role;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.Update(user);

            return BaseResponseModel<Guid>.SuccessResponse(user.Id, "User updated successfully.");

        }
        catch (Exception)
        {

            return BaseResponseModel<Guid>.FailureResponse("An error occurred while updating the user.");
        }
    }

    public async Task<BaseResponseModel<bool>> DeleteUserAsync(Guid id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            {
                if (user == null)
                {
                    return BaseResponseModel<bool>.FailureResponse("User not found.");
                }

                await _userRepository.Remove(user);

                return BaseResponseModel<bool>.SuccessResponse(true, "User deleted successfully.");
            }
        }
        catch (Exception)
        {

            return BaseResponseModel<bool>.FailureResponse("An error occurred while deleting the user.");
        }
    }



}
