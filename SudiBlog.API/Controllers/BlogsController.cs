namespace SudiBlog.API.Controllers;
public class BlogsController (ILogger<BlogsController> _logger) : BaseApiController
{
    private readonly ILogger<BlogsController> logger = _logger;

}