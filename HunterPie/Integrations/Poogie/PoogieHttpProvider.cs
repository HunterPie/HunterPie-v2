using HunterPie.Core.Client;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Features.Repository;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Networking.Http;
using HunterPie.Core.Vault;
using System;

namespace HunterPie.Integrations.Poogie;

/// <summary>
/// HTTP client provider for requesting to HunterPie's backend
/// </summary>
internal class PoogieHttpProvider
{
    private readonly ICredentialVault _credentialVault;
    private readonly ILocalRegistry _localRegistry;
    private readonly IFeatureFlagRepository _featureFlagRepository;

    /// <summary>
    /// The Id of this HunterPie installation, it is sent in every request so it's easier to debug exceptions
    /// </summary>
    private const string CLIENT_ID = "X-Client-Id";

    /// <summary>
    /// HunterPie version
    /// </summary>
    private const string APP_VERSION = "X-App-Version";

    /// <summary>
    /// Type of HunterPie (always v2)
    /// </summary>
    private const string CLIENT_TYPE = "X-HunterPie-Client";

    /// <summary>
    /// HunterPie's default user agent
    /// </summary>
    private const string USER_AGENT = "User-Agent";

    /// <summary>
    /// HunterPie Account's session token
    /// </summary>
    private const string TOKEN = "X-Token";

    /// <summary>
    /// HunterPie supporter token in case the user is a supporter
    /// </summary>
    private const string SUPPORTER = "X-Supporter-Token";

    /// <summary>
    /// Unique identifier that is generated every time HunterPie restarts so it is easier to debug exceptions
    /// </summary>
    private const string SESSION_ID = "X-Session-Id";

    /// <summary>
    /// This HunterPie version
    /// </summary>
    private const string CLIENT_TYPE_VALUE = "v2";

    /// <summary>
    /// Unique identifier for this user's HunterPie session
    /// </summary>
    private static readonly Guid SessionId = Guid.NewGuid();

    private static readonly string[] Hosts = { "https://api.hunterpie.com", "https://mirror.hunterpie.com/mirror" };

    public PoogieHttpProvider(
        ICredentialVault credentialVault,
        ILocalRegistry localRegistry,
        IFeatureFlagRepository featureFlagRepository)
    {
        _credentialVault = credentialVault;
        _localRegistry = localRegistry;
        _featureFlagRepository = featureFlagRepository;
    }

    /// <summary>
    /// Builds the default HttpClient with default parameters for sending requests to HunterPie's backend
    /// </summary>
    /// <returns>A builder that can be used to add extra information</returns>
    public HttpClientBuilder Default()
    {
        string? clientId = _localRegistry.Exists("client_id")
            ? _localRegistry.Get<string>("client_id")
            : "Unknown";

        bool shouldRedirect = _featureFlagRepository.IsEnabled(
            FeatureFlags.FEATURE_REDIRECT_POOGIE
        );

        HttpClientBuilder builder = shouldRedirect
            ? new HttpClientBuilder(ClientConfig.Config.Development.PoogieApiHost)
            : new HttpClientBuilder(Hosts);

        string? token = _credentialVault.Get()?.Password;

        builder
            .WithTimeout(TimeSpan.FromSeconds(10))
            .WithRetry(3)
            .WithHeader(CLIENT_ID, clientId)
            .WithHeader(SUPPORTER, ClientConfig.Config.Client.SupporterSecretToken)
            .WithHeader(APP_VERSION, ClientInfo.Version.ToString())
            .WithHeader(CLIENT_TYPE, CLIENT_TYPE_VALUE)
            .WithHeader(SESSION_ID, SessionId.ToString())
            .WithHeader(USER_AGENT, GetUserAgent());

        if (token is { })
            builder.WithHeader(TOKEN, token);

        return builder;
    }

    private static string GetUserAgent()
    {
        string clientVersion = ClientInfo.Version.ToString();
        string platformVersion = Environment.OSVersion.ToString();
        return $"HunterPie/{clientVersion} ({platformVersion})";
    }
}