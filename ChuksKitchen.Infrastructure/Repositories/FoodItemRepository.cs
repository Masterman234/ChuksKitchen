using ChuksKitchen.Application.Interfaces.IRepositories;
using ChuksKitchen.Domain.Entities;
using ChuksKitchen.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChuksKitchen.Infrastructure.Repositories;

public class FoodItemRepository : IFoodItemRepository
{
    private readonly AppDbContext _context;

    public FoodItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FoodItem?> GetByIdAsync(Guid id)
    {
        return await _context.FoodItems
            .Include(f => f.Creator)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<FoodItem>> GetAllAsync()
    {
        return await _context.FoodItems
            .Include(f => f.Creator)
            .ToListAsync();
    }

    public async Task<IEnumerable<FoodItem>> GetAvailableAsync()
    {
        return await _context.FoodItems
            .Where(f => f.IsAvailable)
            .Include(f => f.Creator)
            .ToListAsync();
    }

    public async Task AddAsync(FoodItem foodItem)
    {
        await _context.FoodItems.AddAsync(foodItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(FoodItem foodItem)
    {
        _context.FoodItems.Update(foodItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(FoodItem foodItem)
    {
        _context.FoodItems.Remove(foodItem);
        await _context.SaveChangesAsync();
    }
}
