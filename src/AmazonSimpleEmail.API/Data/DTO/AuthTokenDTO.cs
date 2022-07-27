namespace AmazonSimpleEmail.API.Data.DTO;

public class UserTokenDTO
{
    public string Id { get; set; }
    public string UserName { get; set; }
}

public class AuthTokenDTO
{
    public bool Authenticated { get; set; }
    public DateTime Expiration { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string Message { get; set; }
    public UserTokenDTO UserToken { get; set; }
}

