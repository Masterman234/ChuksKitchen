using ChuksKitchen.Application.Interfaces.IRepositories;
using ChuksKitchen.Domain.Entities;
using ChuksKitchen.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChuksKitchen.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order?> GetOrderWithItemsAsync(Guid orderId)
    {
        return await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.FoodItem)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<List<Order>> GetUserOrdersAsync(Guid userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .ThenInclude(o => o.FoodItem)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
}
