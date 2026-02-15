using ChuksKitchen.Application.Dtos.UserDtos;

namespace ChuksKitchen.Application.Interfaces.IServices;

public interface IUserService
{
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto);
        Task UpdateUserAsync(Guid id, UserDto userUpdateDto);
        Task DeleteUserAsync(Guid id);
}
