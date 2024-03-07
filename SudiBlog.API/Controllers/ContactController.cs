namespace SudiBlog.API.Controllers;
public class ContactController (IEmailService emailService) : BaseApiController
{
    private readonly IEmailService _emailService = emailService;

    [HttpPost]
    public async Task<IActionResult> Contact([FromBody] ContactMe model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            model.Message = $"{model.Message} <hr/> Phone: {model.Phone}";
            await _emailService.SendContactEmailAsync(model.Email, model.Name, model.Subject, model.Message);
            return Ok(new { message = "Email sent successfully!" });
        }
        catch (Exception ex)
        {
            // Log the exception (if logging is set up)
            return StatusCode(500, new { message = $"An error occurred while sending the email: {ex.Message}" });
        }
    }
}
