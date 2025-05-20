using Brainrot.Data;
using Brainrot.Interface;
using Brainrot.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brainrot.Repository;

public class UserRepository : IUserRepository
{
    private readonly BrainrotDbContext _context;
    public UserRepository(BrainrotDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<User>> GetAllUserAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        return await _context.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<bool> AddUserAsync(User user)
    {
        //Check if username already exists
        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
        {
            return false;
        }
        
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = null;
        await _context.Users.AddAsync(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateUserAsync(int userId, User user)
    {
        var userDomain = await _context.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();

        if (userDomain == null)
        {
            return false;
        }

        if (!string.IsNullOrEmpty(user.Username) && user.Username != userDomain.Username)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                return false;
            }

            userDomain.Username = user.Username;
        }
        
        userDomain.Email = user.Email ?? userDomain.Email;
        userDomain.UpdatedAt = DateTime.UtcNow;
        
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var userDomain = await _context.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();

        if (userDomain == null)
        {
            return false;
        }
        
        _context.Users.Remove(userDomain);
        return await _context.SaveChangesAsync() > 0;
    }
}