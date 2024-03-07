namespace SudiBlog.API.DTOs;
public class PostDetailsDto
{
    public Post Post { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
}