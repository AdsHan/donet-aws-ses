namespace AmazonSimpleEmail.API.Application.InputModels;

public class CreateUserInputModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}