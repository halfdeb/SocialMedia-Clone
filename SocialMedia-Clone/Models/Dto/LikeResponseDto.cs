namespace Brainrot.Models.Dto;

public class LikeResponseDto
{
    public int LikeId { get; set; }
    public int UserId { get; set; }
    public int? CommentId { get; set; }
    public int? PostId { get; set; }
    public DateTime CreatedAt { get; set; }
}