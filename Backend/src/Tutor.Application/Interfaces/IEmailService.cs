using System.Threading.Tasks;

namespace Tutor.Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string htmlContent);

}