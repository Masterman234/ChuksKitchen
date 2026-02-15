using System.Threading.Tasks;
using ChuksKitchen.Application.Interfaces.IRepositories;
using ChuksKitchen.Domain.Entities;
using ChuksKitchen.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChuksKitchen.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
      .FirstOrDefaultAsync(u => u.Id == id);

    }


    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
       

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
       
    }
        

    public async Task Update(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
  
       
    }
         

    public async Task Remove(User user)
    {
      _context.Users.Remove(user);
        await _context.SaveChangesAsync();
   
    }

}

