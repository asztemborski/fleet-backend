﻿using Fleet.Modules.Identity.Application.Contracts;
using MediatR;

namespace Fleet.Modules.Identity.Application.Commands.RefreshTokensCommand;

internal sealed class RefreshTokensCommandHandler : IRequestHandler<RefreshTokensCommand>
{
    private readonly ITokensProvider _tokensProvider;
    private readonly ITokensRequestStorage _tokensRequestStorage;

    public RefreshTokensCommandHandler(ITokensProvider tokensProvider, ITokensRequestStorage tokensRequestStorage)
    {
        _tokensProvider = tokensProvider;
        _tokensRequestStorage = tokensRequestStorage;
    }

    public async Task Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
    {
        var tokens = await _tokensProvider.RefreshAsync(request.RefreshToken);
        _tokensRequestStorage.SetTokens(request.RefreshToken, tokens);
    }
}