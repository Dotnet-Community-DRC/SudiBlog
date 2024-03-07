namespace SudiBlog.API.Controllers;
public class CommentsController(ApplicationDbContext context, UserManager<BlogUser> userManager) : BaseApiController
{
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<BlogUser> _userManager = userManager;

       
        [HttpGet("Original")]
        public async Task<IActionResult> Original()
        {
            var originalComments = await _context.Comments.ToListAsync();
            return Ok(originalComments);
        }

        [HttpGet("Moderated")]
        public async Task<IActionResult> Moderated()
        {
            var moderatedComments = await _context.Comments.Where(c => c.Moderated != null).ToListAsync();
            return Ok(moderatedComments);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.BlogUserId = _userManager.GetUserId(User);
                comment.Created = System.DateTime.Now;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var existingComment = await _context.Comments.FindAsync(id);
                if (existingComment == null)
                {
                    return NotFound();
                }

                existingComment.Body = comment.Body;
                existingComment.Updated = DateTime.Now;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
}
