using AmazonSimpleEmail.API.Application.Services.Interface;
using AmazonSimpleEmail.API.Data.DTO;
using AmazonSimpleEmail.API.Data.Entities;
using AmazonSimpleEmail.API.Utilis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AmazonSimpleEmail.API.Application.Services.Implementations;

public class TokenService : ITokenService
{
    private readonly TokenSettings _tokenSettings;
    private readonly IDistributedCache _cache;

    public TokenService(IDistributedCache cache, TokenSettings tokenSettings)
    {
        _cache = cache;
        _tokenSettings = tokenSettings;
    }

    public async Task<AuthTokenDTO> GenerateTokenAsync(string userName)
    {
        // Define as claims do usuário (não é obrigatório mas cria mais chaves no Payload)
        var claims = new[]
        {
             new Claim(JwtRegisteredClaimNames.UniqueName, userName),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Gera uma chave
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretJWTKey));

        // Gera a assinatura digital do token
        var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Tempo de expiracão do token            
        var expiration = DateTime.UtcNow.AddSeconds(_tokenSettings.ExpireSeconds);

        // Monta as informações do token
        JwtSecurityToken token = new JwtSecurityToken(
          issuer: _tokenSettings.Issuer,
          audience: _tokenSettings.Audience,
          claims: claims,
          expires: expiration,
          signingCredentials: credenciais);

        // Retorna o token e demais informações
        var response = new AuthTokenDTO
        {
            Authenticated = true,
            Expiration = expiration,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = Guid.NewGuid().ToString().Replace("-", String.Empty),
            Message = "Token JWT OK",
            UserToken = new UserTokenDTO
            {
                UserName = userName
            }
        };

        // Armazena o refresh token em cache através do Redis 
        var refreshTokenModel = new RefreshTokenModel()
        {
            RefreshToken = response.RefreshToken,
            UserName = response.UserToken.UserName
        };

        // Validade do refresh token (O Redis irá invalidar o registro automaticamente de acordo com a validade)            
        TimeSpan finalExpiration = TimeSpan.FromSeconds(_tokenSettings.FinalExpirationSeconds);

        DistributedCacheEntryOptions optionsCache = new DistributedCacheEntryOptions();
        optionsCache.SetAbsoluteExpiration(finalExpiration);

        // Grava o Refresh Token no cache
        _cache.SetString(response.RefreshToken, JsonSerializer.Serialize(refreshTokenModel), optionsCache);

        return response;
    }
}
