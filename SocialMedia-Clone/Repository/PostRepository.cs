using Brainrot.Data;
using Brainrot.Interface;
using Brainrot.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brainrot.Repository;

public class PostRepository : IPostRepository
{
    private readonly BrainrotDbContext _context;
    public PostRepository(BrainrotDbContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<Post>> GetAllPostAsync()
    {
        return await _context.Posts
            .Include(p => p.Images)
            .Include(p => p.Comments)
            .Select(p => new
            {
                Post = p,
                LikeCount = _context.Likes.Count(l => l.PostId == p.PostId)
            })
            .ToListAsync()
            .ContinueWith(task => task.Result.Select(x => { x.Post.LikeCount = x.LikeCount; return x.Post; }).ToList());
    }

    public async Task<Post> GetPostByIdAsync(int postId)
    {
        return await _context.Posts.Where(p => p.PostId == postId)
            .Include(p => p.Images)
            .Include(p => p.Comments)
            .Select(p => new
            {
                Post = p,
                LikeCount = _context.Likes.Count(l => l.PostId == p.PostId)
            })
            .FirstOrDefaultAsync()
            .ContinueWith(task =>
            {
                if (task.Result != null)
                    task.Result.Post.LikeCount = task.Result.LikeCount;
                return task.Result?.Post;
            });
    }

    public async Task<Post> GetPostByUserIdAsync(int userId)
    {
        return await _context.Posts.Where(p => p.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task AddPostAsync(Post post)
    {
        post.CreatedAt = DateTime.UtcNow;
        post.UpdatedAt = null;
        post.LikeCount = 0;

        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePostAsync(int postId)
    {
        var DomainModel = await _context.Posts.Where(p => p.PostId == postId).FirstOrDefaultAsync();
        if (DomainModel != null)
        {
            _context.Posts.Remove(DomainModel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdatePostAsync(int postId, Post post)
    {
        var DomainModel = await _context.Posts.Where(p => p.PostId == postId).FirstOrDefaultAsync();
        if (DomainModel != null)
        {
            DomainModel.UpdatedAt = DateTime.UtcNow;
            DomainModel.Caption = post.Caption ?? DomainModel.Caption;

            await _context.SaveChangesAsync();
        }
    }
}