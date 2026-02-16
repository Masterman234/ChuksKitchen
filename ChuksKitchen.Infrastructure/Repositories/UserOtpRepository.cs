using ChuksKitchen.Application.Interfaces.IRepositories;
using ChuksKitchen.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChuksKitchen.Infrastructure.Repositories;

public class UserOtpRepository : IUserOtpRepository
{
    private readonly AppDbContext _context;

    public UserOtpRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserOtp?> GetValidOtpAsync(Guid userId, string code)
    {
        return await _context.UserOtps
            .Where(x => x.UserId == userId
                        && x.Code == code
                        && !x.IsUsed
                        && x.ExpiresAt > DateTime.UtcNow)
            .FirstOrDefaultAsync();
    }

    public async Task<UserOtp?> GetLatestOtpByUserIdAsync(Guid userId)
    {
        return await _context.UserOtps
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(UserOtp otp)
    {
        await _context.UserOtps.AddAsync(otp);
        await _context.SaveChangesAsync();
    }

    public async Task Update(UserOtp otp)
    {
         _context.UserOtps.Update(otp);
      await _context.SaveChangesAsync();
    }

    public async Task InvalidatePreviousOtpsAsync(Guid userId)
    {
        var otps = await _context.UserOtps
        .Where(o => o.UserId == userId && !o.IsUsed)
        .ToListAsync();

        foreach (var otp in otps)
        {
            otp.IsUsed = true;
        }

        await _context.SaveChangesAsync();
    }
}


