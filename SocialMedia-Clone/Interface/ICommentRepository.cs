using Brainrot.Models.Domain;

namespace Brainrot.Interface;

public interface ICommentRepository
{
    Task<ICollection<Comment>> GetAllCommentsAsync();
    Task<Comment> GetCommentByCommentIdAsync(int id);
    Task<ICollection<Comment>> GetCommentByPostIdAsync(int id);
    Task AddCommentAsync(Comment comment);
    Task UpdateCommentAsync(Comment comment, int commentId);
    Task DeleteCommentAsync(int commentId);
}