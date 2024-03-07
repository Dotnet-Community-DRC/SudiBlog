namespace SudiBlog.API.Entities;
public class Post
{
    public int Id { get; set; }
    [Display(Name = "Blog Name")]
    public int BlogId { get; set; }
    public string BlogUserId { get; set; }
    [Required]
    [StringLength(75, ErrorMessage = "The {0} must be at least {2} and no more than {1} characters.", MinimumLength = 3)]
    public string Title { get; set; }
    [Required]
    [StringLength(200, ErrorMessage = "The {0} must be at least {2} and no more than {1} characters.", MinimumLength = 3)]
    public string Abstract { get; set; }
    [Required]
    public string Content { get; set; }
    [Display(Name = "Created Date")]
    [DataType(DataType.Date)]
    public DateTime Created { get; set; } = DateTime.UtcNow;
    [Display(Name = "Updated Date")]
    [DataType(DataType.Date)]
    public DateTime? Updated { get; set; } = DateTime.UtcNow;
    public ReadyStatus ReadyStatus { get; set; }
    public string Slug { get; set; }
    public byte[] ImageData { get; set; }
    [Display(Name = "Image Type")]
    public string ContentType { get; set; }
    [NotMapped]
    public IFormFile Image { get; set; }

    //Navigation properties
    public virtual Blog Blog { get; set; }
    public virtual BlogUser BlogUser { get; set; }
    public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}