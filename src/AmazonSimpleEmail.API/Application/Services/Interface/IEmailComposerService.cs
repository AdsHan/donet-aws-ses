namespace AmazonSimpleEmail.API.Application.Services.Interface;

public interface IEmailComposerService
{
    Task SendAlreadyCreatedAsync(string email);
    Task SendVerificationAsync(string token, string email);
    Task SendPasswordResetAsync(string token, string email);
}

