using System.Linq.Expressions;
using ChuksKitchen.Domain.Common;
using ChuksKitchen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChuksKitchen.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserOtp> UserOtps => Set<UserOtp>();
    public DbSet<FoodItem> FoodItems => Set<FoodItem>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1️⃣ Enforce One Cart Per User
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithOne(u => u.Cart)
            .HasForeignKey<Cart>(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cart>()
            .HasIndex(c => c.UserId)
            .IsUnique();

        // 2️⃣ Composite Unique Index for CartItem (Cart + FoodItem)
        modelBuilder.Entity<CartItem>()
            .HasIndex(ci => new { ci.CartId, ci.FoodItemId })
            .IsUnique();

        // 3️⃣ FoodItem → Creator (User)
        modelBuilder.Entity<FoodItem>()
            .HasOne(f => f.Creator)
            .WithMany()
            .HasForeignKey(f => f.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // 4️⃣ Global Soft Delete Filter
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(GetIsDeletedRestriction(entityType.ClrType));
            }
        }
    }

    private static LambdaExpression GetIsDeletedRestriction(Type type)
    {
        var param = Expression.Parameter(type, "e");
        var prop = Expression.Property(param, nameof(BaseEntity.IsDeleted));
        var condition = Expression.Equal(prop, Expression.Constant(false));
        return Expression.Lambda(condition, param);
    }
}
