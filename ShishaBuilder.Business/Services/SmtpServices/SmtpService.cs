using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

public class SmtpService : ISmtpService
{
    private readonly SmtpSettings settings;

    public SmtpService(IOptions<SmtpSettings> options)
    {
        settings = options.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(settings.Host, settings.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(settings.Username, settings.Password);

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(settings.From));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html) { Text = body };

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
