using AmazonSimpleEmail.API.Application.InputModels;
using AmazonSimpleEmail.API.Utilis;

namespace AmazonSimpleEmail.API.Application.Services.Interface;

public interface IUserService
{
    Task<BaseResult> CreateUserAsync(CreateUserInputModel input);
    Task<BaseResult> VerifyEmailAsync(VerifyEmailInputModel input);
    Task<BaseResult> ForgotPasswordAsync(ForgotPasswordInputModel input);
    BaseResult ValidateResetTokenAsync(ValidateResetTokenInputModel input);
    Task<BaseResult> ResetPasswordAsync(ResetPasswordInputModel input);
}

