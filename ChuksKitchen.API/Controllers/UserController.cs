using ChuksKitchen.Application.Common;
using ChuksKitchen.Application.Dtos.UserDtos;
using ChuksKitchen.Application.Dtos.UserOtpDtos;
using ChuksKitchen.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ChuksKitchen.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // Signup endpoint
    [HttpPost("signup")]
    [ProducesResponseType(typeof(BaseResponseModel<UserDto>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<UserDto>), 400)]
    public async Task<IActionResult> SignUp([FromBody] UserCreateDto request)
    {
        var result = await _userService.SignUpAsync(request);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    // OTP verification endpoint
    [HttpPost("verify-otp")]
    [ProducesResponseType(typeof(BaseResponseModel<bool>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<bool>), 400)]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        var result = await _userService.VerifyOtpAsync(request.EmailOrPhone, request.OtpCode);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    // Get user by ID
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponseModel<UserDto?>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<UserDto?>), 404)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return NotFound(result);
    }
    // Update user
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponseModel<Guid>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<Guid>), 400)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateDto request)
    {
        var response = await _userService.UpdateUserAsync(id, request);
        if (!response.Success) return BadRequest(response);
        return Ok(response);
    }

    // Delete user
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponseModel<bool>), 200)]
    [ProducesResponseType(typeof(BaseResponseModel<bool>), 400)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var response = await _userService.DeleteUserAsync(id);
        if (!response.Success) return BadRequest(response);
        return Ok(response);
    }
}

