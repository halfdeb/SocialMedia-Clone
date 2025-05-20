using Brainrot.Models.Domain;

namespace Brainrot.Interface;

public interface IUserRepository
{
    Task<ICollection<User>> GetAllUserAsync();
    Task<User> GetUserByIdAsync(int userId);
    Task<bool> AddUserAsync(User user);
    Task<bool> UpdateUserAsync(int userId, User user);
    Task<bool> DeleteUserAsync(int userId);
}