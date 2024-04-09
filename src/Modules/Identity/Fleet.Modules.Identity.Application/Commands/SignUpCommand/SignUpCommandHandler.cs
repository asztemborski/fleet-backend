﻿using Fleet.Modules.Identity.Application.Contracts;
using Fleet.Modules.Identity.Application.Exceptions;
using Fleet.Modules.Identity.Application.Options;
using Fleet.Modules.Identity.Domain.Entities;
using Fleet.Modules.Identity.Domain.Repositories;
using Fleet.Modules.Identity.Shared.Events;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fleet.Modules.Identity.Application.Commands.SignUpCommand;

internal sealed class SignUpCommandHandler : IRequestHandler<SignUpCommand>
{
    private readonly IPublisher _publisher;
    private readonly IUserRepository _userRepository;
    private readonly IValueHasher _valueHasher;
    private readonly EmailOptions _emailOptions;

    public SignUpCommandHandler(IUserRepository userRepository, IValueHasher valueHasher, IPublisher publisher,
        IOptions<EmailOptions> emailOptions)
    {
        _userRepository = userRepository;
        _valueHasher = valueHasher;
        _publisher = publisher;
        _emailOptions = emailOptions.Value;
    }

    public async Task Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var provider = request.Email.Split("@").Last();

        if (_emailOptions.BannedEmailProviders.Any(x => provider.Contains(x)))
        {
            throw new InvalidEmailProviderException(provider);
        }

        if (!await _userRepository.IsEmailUniqueAsync(request.Email))
        {
            throw new EmailAlreadyUsedException(request.Email);
        }

        if (!await _userRepository.IsUsernameUniqueAsync(request.Username))
        {
            throw new UsernameAlreadyUsedException(request.Username);
        }

        var hashedPassword = _valueHasher.Hash(request.Password);
        var user = new IdentityUser(request.Email, request.Username, hashedPassword);
        var createdUser = await _userRepository.CreateAsync(user);

        await _publisher.Publish(new IdentityUserCreatedEvent(createdUser.Email.Address,
            createdUser.Username), cancellationToken);
    }
}