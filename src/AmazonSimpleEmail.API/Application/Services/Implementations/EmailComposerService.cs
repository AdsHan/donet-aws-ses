using AmazonSimpleEmail.API.Application.Services.Interface;
using AmazonSimpleEmail.API.Email;
using AmazonSimpleEmail.API.Utilis;

namespace AmazonSimpleEmail.API.Application.Services.Implementations;

public class EmailComposerService : BaseService, IEmailComposerService
{
    private const string URL_BASE = "http://localhost:5000";

    private readonly IEmailService _email;

    public EmailComposerService(IEmailService email)
    {
        _email = email;
    }

    public async Task SendAlreadyCreatedAsync(string email)
    {
        var message = $@"
                <p>Se você esqueceu a sua senha, você pode redefini-la através do link <a href=""{URL_BASE}/forgot-password"">forgot password</a> page.</p>";

        await _email.SendAsync(
            emailTo: email,
            subject: "AWS Simple Email Service API - Email Já Cadastrado",
            message: $@"<h4>E-mail Já Registrado</h4>
                     <p>Seu email <strong>{email}</strong> já foi registrado anteriormente.</p>
                     {message}"
        );
    }

    public async Task SendPasswordResetAsync(string token, string email)
    {
        var url = $"{URL_BASE}/reset-password?token={token}";

        var message = $@"
                <p>Por favor clique no link abaixo para redefinir sua senha, o link será válido por 1 dia:</p>
                <p><a href=""{url}"">{url}</a></p>";

        await _email.SendAsync(
            emailTo: email,
            subject: "AWS Simple Email Service API - Redefinir Senha",
            message: $@"<h4>Email de redefinição de senha</h4>
                     {message}"
        );

    }

    public async Task SendVerificationAsync(string token, string email)
    {
        var url = $"{URL_BASE}/verify-email?token={token}";

        var message = $@"
                <p>Clique no link abaixo para verificar seu endereço de e-mail:</p>
                <p><a href=""{url}"">{url}</a></p>";

        await _email.SendAsync(
            emailTo: email,
            subject: "AWS Simple Email Service API - Verificação de Email",
            message: $@"<h4>Verificar E-mail</h4>
                     <p>Obrigado por se registrar!</p>
                     {message}"
        );

    }
}
