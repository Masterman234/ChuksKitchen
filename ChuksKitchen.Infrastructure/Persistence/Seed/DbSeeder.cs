
using ChuksKitchen.Domain.Entities;
using ChuksKitchen.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace ChuksKitchen.Infrastructure.Persistence.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!context.Users.Any(u => u.Role == UserRole.Admin))
        {
            var admin = new User
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                FirstName = "System",
                LastName = "Admin",
                Email = "admin@chukskitchen.com",
                Role = UserRole.Admin,
                ReferralCode = "ADMIN001",
                IsVerified = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var customer = new User
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                FirstName = "Sample",
                LastName = "Customer",
                Email = "customer@chukskitchen.com",
                Role = UserRole.Customer,
                ReferralCode = "CUST001",
                IsVerified = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await context.Users.AddRangeAsync(admin, customer);
            await context.SaveChangesAsync();
        }
    }
}