using AmazonSimpleEmail.API.Application.InputModels;
using FluentValidation;

namespace AmazonSimpleEmail.API.Application.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserInputModel>
{
    public CreateUserValidator()
    {
        RuleFor(c => c.UserName)
            .NotEmpty()
                .WithMessage("O UserName não foi informado!");

        RuleFor(m => m.Password)
            .NotEmpty()
                .WithMessage("A Senha não foi informada!");

        RuleFor(m => m.Email)
            .NotEmpty()
                .WithMessage("O Email não foi informado!");
    }
}
