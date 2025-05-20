using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Brainrot.Models.Domain;

public class Image
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ImageId { get; set; }
    public int? PostId { get; set; }

    public int UserId { get; set; }
    
    [NotMapped]
    public IFormFile File { get; set; }
    public string FileName { get; set; }
    public string? FileDescription { get; set; }
    public string FileExtension { get; set; }
    public long FileSizeInBytes { get; set; }
    public string FilePath { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    //Navigation Properties 
    [JsonIgnore]
    public User User { get; set; }
    [JsonIgnore]
    public Post? Post { get; set; }
}