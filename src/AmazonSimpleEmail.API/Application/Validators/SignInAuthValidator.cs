using AmazonSimpleEmail.API.Application.InputModels;
using FluentValidation;

namespace AmazonSimpleEmail.API.Application.Validators;

public class SignInAuthValidator : AbstractValidator<SignInAuthInputModel>
{
    public SignInAuthValidator()
    {
        RuleFor(c => c.UserName)
            .NotEmpty()
                .WithMessage("O UserName não foi informado!");

        RuleFor(m => m.Password)
            .NotEmpty()
                .When(m => string.IsNullOrEmpty(m.RefreshToken))
                .WithMessage("A Senha não foi informada!");

        RuleFor(m => m.RefreshToken)
            .Empty()
                .When(m => !string.IsNullOrEmpty(m.Password))
                .WithMessage("Informe a Senha para login ou o Refresh Token para obter um novo token de acesso!");
    }
}
