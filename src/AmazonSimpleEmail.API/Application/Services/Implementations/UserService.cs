using AmazonSimpleEmail.API.Application.InputModels;
using AmazonSimpleEmail.API.Application.Services.Interface;
using AmazonSimpleEmail.API.Data.Entities;
using AmazonSimpleEmail.API.Utilis;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace AmazonSimpleEmail.API.Application.Services.Implementations;

public class UserService : BaseService, IUserService
{
    private readonly IEmailComposerService _emailComposer;
    private readonly IDistributedCache _cache;
    private readonly UserManager<UserModel> _userManager;

    public UserService(IDistributedCache cache, UserManager<UserModel> userManager, IEmailComposerService emailComposer) : base()
    {
        _cache = cache;
        _userManager = userManager;
        _emailComposer = emailComposer;
    }

    public async Task<BaseResult> CreateUserAsync(CreateUserInputModel input)
    {
        var userDB = await _userManager.FindByNameAsync(input.UserName);

        if (userDB is not null)
        {
            await _emailComposer.SendAlreadyCreatedAsync(userDB.Email);

            AddError("Usuário já cadastrado, verifique seu e-mail para obter instruções!");

            return result;
        }

        var user = new UserModel()
        {
            UserName = input.UserName,
            Email = input.Email,
            EmailConfirmed = false
        };

        var identityResult = await _userManager.CreateAsync(user, input.Password);

        if (!identityResult.Succeeded)
        {
            AddError("Não foi possível incluir o usuário!");
            return result;
        }

        var token = GetTokenEmailString(user);

        await _emailComposer.SendVerificationAsync(token, user.Email);

        result.Response = token;

        return result;
    }

    public async Task<BaseResult> VerifyEmailAsync(VerifyEmailInputModel input)
    {
        var token = _cache.GetString(input.Token);

        if (token is null)
        {
            AddError("Não foi possível localizar o token!");
            return result;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var tokenModel = JsonSerializer.Deserialize<EmailTokenModel>(token, options);

        var userDB = await _userManager.FindByNameAsync(tokenModel.UserName);
        userDB.EmailConfirmed = true;

        var identityResult = await _userManager.UpdateAsync(userDB);
        if (!identityResult.Succeeded)
        {
            AddError("Não foi possível alterar o usuário!");
            return result;
        }

        return result;
    }

    public async Task<BaseResult> ForgotPasswordAsync(ForgotPasswordInputModel input)
    {
        var userDB = await _userManager.FindByNameAsync(input.UserName);

        if (userDB is null)
        {
            AddError("Não foi possível localizar o usuário!");
            return result;
        }

        var token = GetTokenEmailString(userDB);

        await _emailComposer.SendPasswordResetAsync(token, userDB.Email);

        result.Response = token;

        return result;
    }

    public BaseResult ValidateResetTokenAsync(ValidateResetTokenInputModel input)
    {
        var token = _cache.GetString(input.Token);

        if (token is null)
        {
            AddError("Não foi possível localizar o token!");
            return result;
        }

        return result;
    }

    public async Task<BaseResult> ResetPasswordAsync(ResetPasswordInputModel input)
    {
        var token = _cache.GetString(input.Token);

        if (token is null)
        {
            AddError("Não foi possível localizar o token!");
            return result;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var tokenModel = JsonSerializer.Deserialize<EmailTokenModel>(token, options);

        var userDB = await _userManager.FindByNameAsync(tokenModel.UserName);

        var identityResult = await _userManager.ChangePasswordAsync(userDB, userDB.PasswordHash, input.Password);

        if (!identityResult.Succeeded)
        {
            AddError("Não foi possível redefinir a senha do usuário!");
            return result;
        }

        return result;
    }

    private string GetTokenEmailString(UserModel user)
    {
        var token = Guid.NewGuid().ToString().Replace("-", String.Empty);

        var resetTokenModel = new EmailTokenModel()
        {
            ResetToken = token,
            UserName = user.UserName,
            Email = user.Email
        };

        TimeSpan finalExpiration = TimeSpan.FromHours(1);

        DistributedCacheEntryOptions optionsCache = new DistributedCacheEntryOptions();
        optionsCache.SetAbsoluteExpiration(finalExpiration);

        _cache.SetString(resetTokenModel.ResetToken, JsonSerializer.Serialize(resetTokenModel), optionsCache);

        return token;
    }
}
