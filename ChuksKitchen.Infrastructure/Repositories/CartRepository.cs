using System.Threading.Tasks;
using ChuksKitchen.Application.Interfaces.IRepositories;
using ChuksKitchen.Domain.Entities;
using ChuksKitchen.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChuksKitchen.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByIdAsync(Guid id)
    {
        return await _context.Carts.FindAsync(id);
    }


    public async Task<Cart?> GetCartWithItemsAsync(Guid userId)
    {
        return await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.FoodItem)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task AddAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }


    public async Task Update(Cart cart)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
    }


    public async Task Remove(Cart cart)
    {
        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}


