using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;

namespace Tutor.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly string _apiKey;

    public EmailService(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string htmlContent)
    {
        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress("tutormd.online@gmail.com", "TutorMD");
        var msg = MailHelper.CreateSingleEmail(from, new EmailAddress(to), subject, "", htmlContent);

        var response = await client.SendEmailAsync(msg);

        // Check if the request succeeded (status code 2xx)
        if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
        {
            return true;
        }
        else
        {
            var responseBody = await response.Body.ReadAsStringAsync();
            return false;
        }
    }

}