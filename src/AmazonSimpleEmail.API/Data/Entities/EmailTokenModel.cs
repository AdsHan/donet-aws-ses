namespace AmazonSimpleEmail.API.Data.Entities;

public class EmailTokenModel
{
    public string ResetToken { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}

