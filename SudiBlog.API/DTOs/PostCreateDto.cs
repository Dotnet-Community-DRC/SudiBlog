namespace SudiBlog.API.DTOs;
public class PostCreateDto
{
    public int BlogId { get; set; }
    public string Title { get; set; }
    public string Abstract { get; set; }
    public string Content { get; set; }
    public ReadyStatus ReadyStatus { get; set; }
    public byte[] ImageData { get; set; }
    public List<string> TagValues { get; set; } = [];
}
