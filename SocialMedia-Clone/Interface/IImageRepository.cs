using Brainrot.Models.Domain;

namespace Brainrot.Interface;

public interface IImageRepository
{
    Task<Image> GetImageByIdAsync(int imageId);
    Task<ICollection<Image>> GetAllImagesAsync();
    Task<Image> AddImageAsync(Image image);
    Task DeleteImageAsync(int imageId);
}