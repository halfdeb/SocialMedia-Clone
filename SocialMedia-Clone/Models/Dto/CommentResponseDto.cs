namespace Brainrot.Models.Dto;

public class CommentResponseDto
{
    public int CommentId { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdaedAt { get; set; }
    public int LikeCount { get; set; }
}