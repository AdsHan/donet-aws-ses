using AmazonSimpleEmail.API.Application.InputModels;
using FluentValidation;

namespace AmazonSimpleEmail.API.Application.Validators;

public class ValidateResetTokenValidator : AbstractValidator<ValidateResetTokenInputModel>
{
    public ValidateResetTokenValidator()
    {
        RuleFor(c => c.Token)
            .NotEmpty()
                .WithMessage("O Token não foi informado!");
    }
}
