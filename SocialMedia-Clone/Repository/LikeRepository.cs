using Brainrot.Data;
using Brainrot.Interface;
using Brainrot.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brainrot.Repository;

public class LikeRepository : ILikeRepository
{
    private readonly BrainrotDbContext _context;

    public LikeRepository(BrainrotDbContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<Like>> GetAllLikesAsync()
    {
        return await _context.Likes.ToListAsync();
    }

    public async Task<Like> GetLikeByLikeIdAsync(int likeId)
    {
        return await _context.Likes.Where(l => l.LikeId == likeId).FirstOrDefaultAsync();
    }

    public async Task<bool> AddLikeAsync(Like like)
    {
        var existingLike = await _context.Likes
            .FirstOrDefaultAsync(l =>
                l.UserId == like.UserId && l.PostId == like.PostId || l.CommentId == like.CommentId);

        if (existingLike != null)
        {
            return false;
        }
        
        like.CreatedAt = DateTime.UtcNow;

        await _context.Likes.AddAsync(like);
        return await _context.SaveChangesAsync()>0;
    }

    public async Task DeleteLikeAsync(int likeId)
    {
        var DomainModel = _context.Likes.Where(l =>  l.LikeId == likeId).FirstOrDefault();
        if (DomainModel != null)
        {
            _context.Likes.Remove(DomainModel);
            await _context.SaveChangesAsync();
        }
    }
}