using Brainrot.Models.Domain;

namespace Brainrot.Interface;

public interface IPostRepository
{
    Task<ICollection<Post>> GetAllPostAsync();
    Task<Post> GetPostByIdAsync(int postId);
    Task<Post>GetPostByUserIdAsync(int postId);
    Task AddPostAsync(Post post);
    Task DeletePostAsync(int postId);
    Task UpdatePostAsync(int postId, Post post);
}