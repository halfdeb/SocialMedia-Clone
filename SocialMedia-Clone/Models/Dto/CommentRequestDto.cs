namespace Brainrot.Models.Dto;

public class CommentRequestDto
{
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string Text { get; set; }
}