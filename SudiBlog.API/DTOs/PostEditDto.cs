namespace SudiBlog.API.DTOs;
public class PostEditDto
{
    public int MyProperty { get; set; }
    public string Title { get; set; }
    public string Abstract { get; set; }
    public string Content { get; set; }
    public ReadyStatus ReadyStatus { get; set; }
    public string ImageData { get; set; } 
    public List<string> TagValues { get; set; } = [];
   
}