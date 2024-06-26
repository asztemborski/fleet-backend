﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Thrive.Shared.Infrastructure;

namespace Thrive.Modules.Notifications.Core.Options.Setups;

internal sealed class MailingOptionsSetup : IConfigureOptions<MailingOptions>
{
    private readonly IConfiguration _configuration;

    public MailingOptionsSetup(IConfiguration configuration) => _configuration = configuration;

    public void Configure(MailingOptions options)
    {
        _configuration.GetSection<MailingOptions>().Bind(options);
    }
}