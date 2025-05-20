using Brainrot.Models.Domain;

namespace Brainrot.Interface;

public interface ILikeRepository
{
    Task<ICollection<Like>> GetAllLikesAsync();
    Task<Like> GetLikeByLikeIdAsync(int likeId);
    Task<bool> AddLikeAsync(Like like);
    Task DeleteLikeAsync(int likeId);
}