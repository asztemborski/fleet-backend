﻿using FluentValidation;

namespace Fleet.Modules.Identity.Application.Commands.SignInCommand;

public sealed class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(request => request.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();

        RuleFor(request => request.Password)
            .NotNull()
            .NotEmpty();
    }
}