using Microsoft.Extensions.Options;
using Pds.Core.ApiClient;
using Pds.Core.ApiClient.Exceptions;
using Pds.Core.ApiClient.Interfaces;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Config;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// Exposes methods for interacting with the Organisation API.
    /// </summary>
    public class OrganisationApiClient : BaseApiClient<OrganisationApiClientConfiguration>, IOrganisationApiClient
    {
        private readonly ILoggerAdapter<OrganisationApiClient> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationApiClient"/> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        /// <param name="httpClient">The HttpClient to be used for sending and receiving HTTP requests.</param>
        /// <param name="configurationOptions">The configuration options.</param>
        /// <param name="logger">The logger.</param>
        public OrganisationApiClient(
            IAuthenticationService<OrganisationApiClientConfiguration> authenticationService,
            HttpClient httpClient,
            IOptions<ServicesConfiguration> configurationOptions,
            ILoggerAdapter<OrganisationApiClient> logger)
            : base(authenticationService, httpClient, Options.Create(configurationOptions.Value.OrganisationApiClient))
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Organisation> GetOrganisation(OrganisationIdentifier identifier)
        {
            return await GetWithAADAuth<Organisation>(
                $"/api/organisation/{identifier.Type}/{identifier.Value}");
        }

        /// <inheritdoc/>
        protected override Action<ApiGeneralException> FailureAction =>
            exception =>
            {
                _logger?.LogError(exception, exception.Message);
                base.FailureAction(exception);
            };
    }
}