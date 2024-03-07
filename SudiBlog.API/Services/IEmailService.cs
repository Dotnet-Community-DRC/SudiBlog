using Microsoft.AspNetCore.Identity.UI.Services;

namespace SudiBlog.API.Services
{
    public interface IEmailService : IEmailSender
    {
         Task SendContactEmailAsync(string emailFrom, string name, string subject, string htmlMessage);
    }
}