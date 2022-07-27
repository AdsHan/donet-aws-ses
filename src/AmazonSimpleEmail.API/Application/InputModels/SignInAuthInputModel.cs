namespace AmazonSimpleEmail.API.Application.InputModels;

public class SignInAuthInputModel
{
    public string UserName { get; set; }
    public string? Password { get; set; }
    public string? RefreshToken { get; set; }
}