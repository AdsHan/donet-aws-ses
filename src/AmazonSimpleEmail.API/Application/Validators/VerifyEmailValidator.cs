using AmazonSimpleEmail.API.Application.InputModels;
using FluentValidation;

namespace AmazonSimpleEmail.API.Application.Validators;

public class VerifyEmailValidator : AbstractValidator<VerifyEmailInputModel>
{
    public VerifyEmailValidator()
    {
        RuleFor(c => c.Token)
            .NotEmpty()
                .WithMessage("O Token não foi informado!");
    }
}
