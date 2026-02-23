using ChuksKitchen.Application.Interfaces.IIdentity;
using ChuksKitchen.Application.Interfaces.IRepositories;
using ChuksKitchen.Application.Interfaces.IServices;
using ChuksKitchen.Application.Services;
using ChuksKitchen.Infrastructure.Cloudinarys;
using ChuksKitchen.Infrastructure.Identity;
using ChuksKitchen.Infrastructure.Persistence;
using ChuksKitchen.Infrastructure.Persistence.Seed;
using ChuksKitchen.Infrastructure.Repositories;
using ChuksKitchen.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext using SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Services
builder.Services.AddScoped<IUserOtpService, UserOtpService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, FakeEmailService>();
builder.Services.AddScoped<IFoodItemService, FoodItemService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IRatingService, RatingService>();

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddSingleton<ICloudinaryService,CloudinaryService>();




//Repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserOtpRepository, UserOtpRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IFoodItemRepository, FoodItemRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();

builder.Services.AddScoped<IOtpGenerator, OtpGenerator>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(context);
}

app.Run();
