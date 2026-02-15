using ChuksKitchen.Domain.Common;
using ChuksKitchen.Domain.Entities;

namespace ChuksKitchen.Application.Interfaces.IRepositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task Update(User user);
    Task Remove(User user);
}
