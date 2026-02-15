using ChuksKitchen.Domain.Entities;

namespace ChuksKitchen.Application.Interfaces.IRepositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<Order?> GetOrderWithItemsAsync(Guid orderId);
    Task<List<Order>> GetUserOrdersAsync(Guid userId);
    Task AddAsync(Order order);
    void Update(Order order);
    Task SaveChangesAsync();
}