using Brainrot.Data;
using Brainrot.Interface;
using Brainrot.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brainrot.Repository;

public class ImageRepository : IImageRepository
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly BrainrotDbContext _context;

    public ImageRepository(IWebHostEnvironment webHostEnvironment, 
        IHttpContextAccessor httpContextAccessor,
        BrainrotDbContext context)
    {
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public async Task<ICollection<Image>> GetAllImagesAsync()
    {
        return await _context.Images
            .Include(p => p.User)
            .ToListAsync();
    }
    
    public async Task<Image> GetImageByIdAsync(int imageId)
    {
        return await _context.Images.Where(p => p.ImageId == imageId)
            .Include(p => p.User)
            .FirstOrDefaultAsync();
    }

    public async Task<Image> AddImageAsync(Image image)
    {
        if (image.PostId.HasValue)
        {
            // Associate the image with a post
            var post = await _context.Posts
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.PostId == image.PostId.Value);

            if (post == null)
                throw new ArgumentException("Invalid PostId.");

            // Save the file and generate the URL
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            post.Images.Add(image);
        }
        else
        {
            // Associate the image with a user
            var user = await _context.Users
                .Include(u => u.Images)
                .FirstOrDefaultAsync(u => u.UserId == image.UserId);

            if (user == null)
                throw new ArgumentException("Invalid UserId.");

            // Save the file and generate the URL
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            user.Images.Add(image);
        }

        // Save the changes to the database
        await _context.SaveChangesAsync();

        return image;
    }
        

    public async Task DeleteImageAsync(int imageId)
    {
        var image = await _context.Images.FindAsync(imageId);
        if (image != null)
        {
            //Delete the physical file if it exists
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            if (File.Exists(localFilePath))
            {
                File.Delete(localFilePath);
            }
            
            //Remove the image record from the database
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
}