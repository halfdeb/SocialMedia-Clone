using Brainrot.Models.Domain;

namespace Brainrot.Models.Dto;

public class PostResponseDto
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Caption { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int likeCount { get; set; }
    
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Image> Images { get; set; }
}