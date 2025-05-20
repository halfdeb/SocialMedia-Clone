using Brainrot.Models.Domain;

namespace Brainrot.Models.Dto;

public class ImageResponseDto
{
    public int ImageId { get; set; }
    public int? PostId { get; set; }
    public int UserId { get; set; }
    public string FileName { get; set; }
    public string FileDescription { get; set; }
    public string FilePath { get; set; }
}