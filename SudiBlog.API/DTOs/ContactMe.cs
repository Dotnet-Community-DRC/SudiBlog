namespace SudiBlog.API.DTOs;
public class ContactMe
{
    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 3)]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    [Display(Name = "Email Address")]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 3)]
    public string Email { get; set; }
    [Phone]
    [Display(Name = "Phone Number")]
    public string Phone { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 3)]
    public string Subject { get; set; }
    [Required]
    [Display(Name = "Message")]
    [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 3)]
    public string Message { get; set; }
}