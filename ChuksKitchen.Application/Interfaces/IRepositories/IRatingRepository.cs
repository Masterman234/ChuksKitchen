using ChuksKitchen.Domain.Entities;

namespace ChuksKitchen.Application.Interfaces.IRepositories;

public interface IRatingRepository
{
    Task AddAsync(Rating rating);
    Task<bool> ExistsAsync(Guid userId, Guid foodItemId);
}
