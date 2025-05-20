namespace Brainrot.Models.Domain;

public class Like
{
    public int LikeId { get; set; }
    public int UserId { get; set; }
    public int? CommentId { get; set; }
    public int? PostId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    //Navigation Property
    public User User { get; set; } 
    public Comment Comment { get; set; }
    public Post Post { get; set; }
}