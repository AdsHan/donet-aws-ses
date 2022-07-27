namespace AmazonSimpleEmail.API.Email;

public interface IEmailService
{
    Task SendAsync(string emailTo, string subject, string message);
}

