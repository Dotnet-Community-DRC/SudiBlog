namespace SudiBlog.API.Controllers;
public class BlogsController(ApplicationDbContext context, IImageService imageService, UserManager<BlogUser> userManager) : BaseApiController
{

    private readonly ApplicationDbContext _context = context;
    private readonly IImageService _imageService = imageService;
    private readonly UserManager<BlogUser> _userManager = userManager;


    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> Index()
    {
        var blogs = await _context.Blogs.Include(b => b.BlogUser).ToListAsync();
        return Ok(blogs);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Blog>> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var blog = await _context.Blogs
            .Include(b => b.BlogUser)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (blog == null)
        {
            return NotFound();
        }

        return blog;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<Blog>> Create([FromBody] Blog blog)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        blog.Created = DateTime.Now;
        blog.BlogUserId = _userManager.GetUserId(User);
        if (blog.Image != null)
        {
            blog.ImageData = await _imageService.EncodeImageAsync(blog.Image);
            blog.ContentType = _imageService.ContentType(blog.Image);
        }
        _context.Add(blog);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Details), new { id = blog.Id }, blog);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(int id, [FromBody] Blog blog)
    {
        if (id != blog.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(blog);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(blog.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog == null)
        {
            return NotFound();
        }

        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool BlogExists(int id)
    {
        return _context.Blogs.Any(e => e.Id == id);
    }
}