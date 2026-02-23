using ChuksKitchen.Domain.Common;
using ChuksKitchen.Domain.Entities;

namespace ChuksKitchen.Application.Interfaces.IRepositories;

public interface IUserRepository
{
    Task<List<User?>> GetAllUsersAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByPhone(string phone);
    Task<User?> GetByIdWithOrdersAsync(Guid id);
    Task<User?> GetByReferralCodeAsync(string code);
    Task AddAsync(User user);
    Task Update(User user);
    Task Remove(User user);
   

}
