using AmazonSimpleEmail.API.Application.InputModels;
using FluentValidation;

namespace AmazonSimpleEmail.API.Application.Validators;

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordInputModel>
{
    public ForgotPasswordValidator()
    {
        RuleFor(c => c.UserName)
            .NotEmpty()
                .WithMessage("O UserName não foi informado!");
    }
}
