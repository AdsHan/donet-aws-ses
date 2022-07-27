using AmazonSimpleEmail.API.Application.InputModels;
using AmazonSimpleEmail.API.Utilis;

namespace AmazonSimpleEmail.API.Application.Services.Interface;

public interface IAuthService
{
    Task<BaseResult> SignInAsync(SignInAuthInputModel input);
}
