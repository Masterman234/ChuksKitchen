using ChuksKitchen.Application.Interfaces.IRepositories;
using ChuksKitchen.Domain.Entities;
using ChuksKitchen.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChuksKitchen.Infrastructure.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly AppDbContext _context;

    public RatingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Rating rating)
    {
        await _context.Ratings.AddAsync(rating);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid foodItemId)
    {
        return await _context.Ratings
            .AnyAsync(r => r.UserId == userId && r.FoodItemId == foodItemId);
    }

}