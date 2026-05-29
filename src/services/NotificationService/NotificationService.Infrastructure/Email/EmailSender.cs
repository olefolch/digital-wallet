using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using NotificationService.Application.Interfaces;

namespace NotificationService.Infrastructure.Email;

public class EmailSender(IConfiguration configuration) : IEmailSender
{
    public async Task SendAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(configuration["Email:From"]!));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(configuration["Email:Host"]!, int.Parse(configuration["Email:Port"]!), false);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}