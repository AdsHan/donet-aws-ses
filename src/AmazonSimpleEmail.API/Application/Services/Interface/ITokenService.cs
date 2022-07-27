using AmazonSimpleEmail.API.Data.DTO;

namespace AmazonSimpleEmail.API.Application.Services.Interface;

public interface ITokenService
{
    Task<AuthTokenDTO> GenerateTokenAsync(string userName);
}

