using AmazonSimpleEmail.API.Application.InputModels;
using FluentValidation;

namespace AmazonSimpleEmail.API.Application.Validators;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordInputModel>
{
    public ResetPasswordValidator()
    {
        RuleFor(c => c.Token)
            .NotEmpty()
                .WithMessage("O Token não foi informado!");
        RuleFor(c => c.Password)
            .NotEmpty()
                .WithMessage("A senha não foi informado!");
        RuleFor(c => c.ConfirmPassword)
            .NotEmpty()
                .WithMessage("A confirmação de senha não foi informado!");
    }
}
