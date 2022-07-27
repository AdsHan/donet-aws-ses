namespace AmazonSimpleEmail.API.Application.InputModels;

public class ResetPasswordInputModel
{
    public string Token { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

}