namespace Brainrot.Models.Dto;

public class LikeRequestDto
{
    public int UserId { get; set; }
    public int? CommentId { get; set; }
    public int? PostId { get; set; }
}