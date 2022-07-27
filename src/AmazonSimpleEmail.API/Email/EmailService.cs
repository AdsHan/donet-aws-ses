using AmazonSimpleEmail.API.Utilis;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AmazonSimpleEmail.API.Email;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(EmailSettings emailSettings)
    {
        _emailSettings = emailSettings;
    }

    public async Task SendAsync(string emailTo, string subject, string message)
    {
        // Cria a Mensagem
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_emailSettings.EmailFrom));
        email.To.Add(MailboxAddress.Parse(emailTo));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

        // Envia o Email
        using var smtp = new SmtpClient();
        smtp.Connect(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
        smtp.Authenticate(_emailSettings.SmtpUser, _emailSettings.SmtpPassword);
        //await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }
}