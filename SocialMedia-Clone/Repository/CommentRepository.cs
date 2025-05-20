using Brainrot.Data;
using Brainrot.Interface;
using Brainrot.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brainrot.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly BrainrotDbContext _context;
    private ICommentRepository _commentRepositoryImplementation;

    public CommentRepository(BrainrotDbContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<Comment>> GetAllCommentsAsync()
    {
        return await _context.Comments
            .Select(c => new
            {
                Comment = c,
                LikeCount = _context.Likes.Count(l => l.CommentId == c.CommentId),
            })
            .ToListAsync()
            .ContinueWith(task => task.Result.Select(x => { x.Comment.LikeCount = x.LikeCount; return x.Comment; }).ToList());
    }

    public async Task<Comment> GetCommentByCommentIdAsync(int commentId)
    {
        return await _context.Comments.Where(x => x.CommentId == commentId)
            .Select(c => new
            {
                Comment = c,
                LikeCount = _context.Likes.Count(l => l.CommentId == c.CommentId)
            })
            .FirstOrDefaultAsync()
            .ContinueWith(task =>
            {
                if (task.Result != null)
                    task.Result.Comment.LikeCount = task.Result.LikeCount;
                return task.Result?.Comment;
            });
    }

    public async Task<ICollection<Comment>> GetCommentByPostIdAsync(int postId)
    {
        return await _context.Comments.Where(x => x.PostId == postId).ToListAsync();
    }

    public async Task AddCommentAsync(Comment comment)
    {
        comment.CreatedAt = DateTime.UtcNow;
        comment.UpdatedAt = null;
        comment.LikeCount = 0;

        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCommentAsync(Comment comment, int commentId)
    {
        var DomainModel = await _context.Comments.Where(x => x.CommentId == commentId).FirstOrDefaultAsync();
        if (DomainModel != null)
        {
            DomainModel.UpdatedAt = DateTime.UtcNow;
            DomainModel.Text = comment.Text;

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCommentAsync(int commentId)
    {
        var DomainModel = await _context.Comments.Where(x => x.CommentId == commentId).FirstOrDefaultAsync();
        if (DomainModel != null)
        {
            _context.Comments.Remove(DomainModel);
            await _context.SaveChangesAsync();
        }
    }
}