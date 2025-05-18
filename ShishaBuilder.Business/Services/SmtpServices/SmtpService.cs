using System.Net;
using System.Net;
using System.Net.Mail;
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
        var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
        {
            Credentials = new NetworkCredential("16dbcba3a950c2", "0ab6d531c5ed32"),
            EnableSsl = true,
        };
        client.Send("from@example.com", "to@example.com", "Hello world", "testbody");
        System.Console.WriteLine("Sent");
    }
}
