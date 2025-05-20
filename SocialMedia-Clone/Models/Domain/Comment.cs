using System.Text.Json.Serialization;

namespace Brainrot.Models.Domain;

public class Comment
{
    public int CommentId { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int LikeCount { get; set; }
    
    //Navigation Property
    public User User { get; set; }
    [JsonIgnore]
    public Post Post { get; set; }
    public ICollection<Like> Likes { get; set; }
}