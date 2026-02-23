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

            // Normalize email
            var normalizedEmail = string.IsNullOrWhiteSpace(request.Email)? null: request.Email.Trim().ToLowerInvariant();

            // Ensure at least one identifier is provided
            if (string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Phone))
            {
                return BaseResponseModel<UserDto>.FailureResponse("Either Email or Phone must be provided.");
            }
            // Validate phone
            if (!string.IsNullOrWhiteSpace(request.Phone) && !PhoneValidator.IsValidNigerianPhone(request.Phone))
            {
                return BaseResponseModel<UserDto>.FailureResponse("Invalid Nigerian phone number format.");
            }

            // Check if email exists (only if provided)
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var existByEmail = await _userRepository.GetByEmailAsync(request.Email);
                if (existByEmail != null)
                {
                    return BaseResponseModel<UserDto>.FailureResponse("Email is already registered.");
                }
            }
            // Check if phone exists (only if provided)
            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                var existByPhone = await _userRepository.GetByPhone(request.Phone);
                if (existByPhone != null)
                {
                    return BaseResponseModel<UserDto>.FailureResponse("Phone number is already registered.");
                }
            }

            // Handle optional referral code
            Guid? referredByUserId = null;
            if (!string.IsNullOrWhiteSpace(request.ReferralCode))
            {
                var referringUser = await _userRepository.GetByReferralCodeAsync(request.ReferralCode);
                if (referringUser == null)
                {
                    return BaseResponseModel<UserDto>.FailureResponse("Invalid referral code.");
                }
                referredByUserId = referringUser.Id;
            }
            // Ensure unique referral code generation
            string referralCode;
            do
            {
                referralCode = ReferralCodeGenerator.Generate();
            }
            while (await _userRepository.GetByReferralCodeAsync(referralCode) != null);

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Role = request.Role,
                ReferralCode = referralCode,
                ReferredByUserId = referredByUserId,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            //Create Cart automatically for the user
            await _cartRepository.AddAsync(new Cart { UserId = user.Id, CreatedAt = DateTime.UtcNow });

            // OTP destination (prefer email)
            string? otpDestination = !string.IsNullOrWhiteSpace(user.Email) ? user.Email : user.Phone;
            if (string.IsNullOrWhiteSpace(otpDestination))
            {
                return BaseResponseModel<UserDto>.FailureResponse("No email or phone available for OTP.");
            } 

            //Generate OTP for email or phone verification via OtpService
            var otpResult = await _userOtpService.GenerateOtpAsync(new CreateUserOtpDto(user.Id, otpDestination));

            if (!otpResult.Success)
            {
                return BaseResponseModel<UserDto>.FailureResponse(otpResult.Message);
            }

            // Map to DTO
            var dto = new UserDto(user.Id, user.FirstName, user.LastName, user.Email, user.Phone, user.ReferralCode, user.ReferredByUserId, user.Role);

            return BaseResponseModel<UserDto>.SuccessResponse(dto, "User registered successfully. Please verify OTP to activate account.");

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
            if (string.IsNullOrWhiteSpace(emailOrPhone))
            {
                return BaseResponseModel<bool>.FailureResponse("Email or phone must be provided.");
            }

            // Find user by email or phone
            User? user = emailOrPhone.Contains("@")
                ? await _userRepository.GetByEmailAsync(emailOrPhone.Trim().ToLowerInvariant())
                : await _userRepository.GetByPhone(emailOrPhone);

            if (user == null)
            {
                return BaseResponseModel<bool>.FailureResponse("User not found.");
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



    public async Task<BaseResponseModel<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userDtos = users.Select(u => new UserDto(u.Id, u.FirstName, u.LastName, u.Email, u.Phone, u.ReferralCode, u.ReferredByUserId, u.Role));

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
            if (user == null)
            {
                return BaseResponseModel<Guid>.FailureResponse("User not found.");
            }


            if (!string.IsNullOrWhiteSpace(request.Email))
            {

                var existing = await _userRepository.GetByEmailAsync(request.Email);
                if (existing != null && existing.Id != id)
                {
                    return BaseResponseModel<Guid>.FailureResponse("Email is already in use.");
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                var existing = await _userRepository.GetByPhone(request.Phone);
                if (existing != null && existing.Id != id)
                {
                    return BaseResponseModel<Guid>.FailureResponse("Phone number is already in use.");
                }
            }

            // Normalize email
            var normalizedEmail = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim().ToLowerInvariant();

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
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
            if (user == null)
            {
                return BaseResponseModel<bool>.FailureResponse("User not found.");
            }

            await _userRepository.Remove(user);

            return BaseResponseModel<bool>.SuccessResponse(true, "User deleted successfully.");

        }
        catch (Exception)
        {

            return BaseResponseModel<bool>.FailureResponse("An error occurred while deleting the user.");
        }
    }

    public async Task<BaseResponseModel<UserDto?>> GetUserByIdAsync(Guid id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return BaseResponseModel<UserDto>.FailureResponse("User not found");
            }
            // Map User entity to UserDto
            var userDto = new UserDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Phone,
                user.ReferralCode,
                user.ReferredByUserId,
                user.Role
                );

            return BaseResponseModel<UserDto>.SuccessResponse(userDto, "User retrieved successfully.");
        }
        catch (Exception)
        {
            return BaseResponseModel<UserDto>.FailureResponse("An error occurred while retrieving the user.");
        }
    }

}
