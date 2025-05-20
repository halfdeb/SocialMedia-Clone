using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brainrot.Models.Domain;

public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Caption { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int LikeCount { get; set; }
    
    //Navigation Properties 
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Image> Images { get; set; }
    public ICollection<Like> Likes { get; set; }
    public User User { get; set; }
}