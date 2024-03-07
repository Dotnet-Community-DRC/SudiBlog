namespace SudiBlog.API.Controllers;
public class PostsController (ApplicationDbContext context, ISlugService slugService, 
                                IImageService imageService, 
                                UserManager<BlogUser> userManager, 
                                SearchService searchService) : BaseApiController
{
    private readonly ApplicationDbContext _context = context;
    private readonly ISlugService _slugService = slugService;
    private readonly IImageService _imageService = imageService;
    private readonly UserManager<BlogUser> _userManager = userManager;
    private readonly SearchService _searchService = searchService;

    
    [HttpGet("Search")]
    [AllowAnonymous]
    public async Task<ActionResult> SearchIndex(int? page, string searchTerm)
    {
        var pageNumber = page ?? 1;
        var pageSize = 5;

        var posts = _searchService.Search(searchTerm);
        return Ok(posts);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Post>>> GetBlogs()
    {
        var posts = await _context.Posts.Include(p => p.Blog).Include(p => p.BlogUser).ToListAsync();
        return posts; 
    }

    [HttpGet("Blog/{id}")]
    [AllowAnonymous]
    public async Task<ActionResult> GetBlog(int? id, int? page)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pageNumber = page ?? 1;
        var pageSize = 5;

        var posts = await _context.Posts
            .Where(p => p.BlogId == id && p.ReadyStatus == ReadyStatus.ProductionReady)
            .OrderByDescending(p => p.Created).ToListAsync();

        return Ok(posts);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<Post>> Create([FromBody] Post post, [FromBody] List<string> tagValues)
    {
        if (ModelState.IsValid)
        {
            // Additional logic to process the post and tags...
            // Skipping detailed implementation for brevity

            return CreatedAtAction(nameof(Details), new { slug = post.Slug }, post); 
        }

        return BadRequest(ModelState); 
    }

    [HttpPut("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [FromBody] Post post, [FromBody] List<string> tagValues)
    {
        if (id != post.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            // Additional logic to update the post and tags...
            // Skipping detailed implementation for brevity

            return NoContent(); 
        }

        return BadRequest(ModelState);
    }

    [HttpGet("{slug}")]
    [AllowAnonymous]
    public async Task<ActionResult<PostDetailsDto>> Details(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var post = await _context.Posts
            .Include(p => p.BlogUser)
            .Include(p => p.Tags)
            .Include(p => p.Comments).ThenInclude(c => c.BlogUser)
            .Include(p => p.Comments).ThenInclude(c => c.Moderator)
            .FirstOrDefaultAsync(m => m.Slug == slug);

        if (post == null) 
        {
            return NotFound();
        }

        var dataDto = new PostDetailsDto
        {
            Post = post,
            Tags = _context.Tags.Select(t => t.Text.ToLower()).Distinct().ToList()
        };

        return dataDto; 
    }

    [HttpDelete("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return NoContent(); 
    }

    private bool PostExists(int id)
    {
        return _context.Posts.Any(e => e.Id == id);
    }
}