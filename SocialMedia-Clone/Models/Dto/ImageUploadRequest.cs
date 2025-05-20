using System.ComponentModel.DataAnnotations;

namespace Brainrot.Models.Dto;

public class ImageUploadRequestDto
{
    [Required]
    public IFormFile File { get; set; }
    
    [Required]
    public string FileName { get; set; }

    public int? PostId { get; set; }
    public int UserId { get; set; }
    public string? FileDescription { get; set; }
}