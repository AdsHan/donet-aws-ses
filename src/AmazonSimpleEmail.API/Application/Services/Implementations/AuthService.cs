using AmazonSimpleEmail.API.Application.InputModels;
using AmazonSimpleEmail.API.Application.Services.Interface;
using AmazonSimpleEmail.API.Data.Entities;
using AmazonSimpleEmail.API.Utilis;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace AmazonSimpleEmail.API.Application.Services.Implementations;

public class AuthService : BaseService, IAuthService
{
    private readonly IDistributedCache _cache;
    private readonly ITokenService _token;
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signInManager;

    public AuthService(IDistributedCache cache, ITokenService token, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager) : base()
    {
        _cache = cache;
        _token = token;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<BaseResult> SignInAsync(SignInAuthInputModel input)
    {
        // Verifica se o usuário existe
        var userDB = await _userManager.FindByNameAsync(input.UserName);

        if (userDB == null)
        {
            AddError("Este usuário não existe!");
            return result;
        }

        if (!string.IsNullOrEmpty(input.Password))
        {
            var signIn = await _signInManager.PasswordSignInAsync(input.UserName, input.Password, isPersistent: false, lockoutOnFailure: true);

            if (!signIn.Succeeded)
            {
                AddError("Usuário ou Senha incorretos!");
                return result;
            }

            if (signIn.IsLockedOut)
            {
                AddError("Usuário temporariamente bloqueado por tentativas inválidas");
                return result;
            }
        }
        else
        {
            if (!String.IsNullOrWhiteSpace(input.RefreshToken))
            {
                RefreshTokenModel refreshTokenModel = null;

                string strTokenArmazenado = _cache.GetString(input.RefreshToken);

                if (!String.IsNullOrWhiteSpace(strTokenArmazenado))
                {
                    refreshTokenModel = JsonSerializer.Deserialize<RefreshTokenModel>(strTokenArmazenado);
                }

                bool accessCredentialsValid = (refreshTokenModel != null && input.UserName == refreshTokenModel.UserName && input.RefreshToken == refreshTokenModel.RefreshToken);

                if (accessCredentialsValid)
                {
                    // Elimina o token de refresh uma vez que será gerado um novo
                    _cache.Remove(input.RefreshToken);

                }
                else
                {
                    AddError("Refresh Token Inválido!");
                    return result;
                }
            }
            else
            {
                AddError("Não foi infornado Refresh Token!");
                return result;
            }
        }

        result.Response = await _token.GenerateTokenAsync(input.UserName);

        return result;
    }
}
