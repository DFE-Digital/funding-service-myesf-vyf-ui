using System;

namespace PDS.ViewYourFunding.Core.Configuration
{
    /// <summary>
    /// The Application configuration class.
    /// </summary>
    public class ApplicationConfiguration
    {
        /// <summary>
        /// The release environment.
        /// </summary>
        private const string ReleaseEnvironment = "release";

        /// <summary>
        /// Gets or sets the 'Home' URL for logged in providers.
        /// </summary>
        public string LoggedInProviderHomeLink { get; set; } = "/";

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the build configuration.
        /// </summary>
        public string BuildConfiguration { get; set; }

        /// <summary>
        /// Gets a value indicating whether the API should be secure using OAuth.
        /// </summary>
        /// <value>
        ///  Set to true if OAuth security is enabled on the API.
        /// </value>
        public bool IsProductionEnvironment =>
            ReleaseEnvironment.Equals(Environment, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Gets or sets the global cache time to live.
        /// </summary>
        /// <value>
        /// The global cache time to live.
        /// </value>
        public int GlobalCacheTimeToLive { get; set; } = 5;

        /// <summary>
        /// Gets or sets the feedback link.
        /// </summary>
        /// <value>
        /// The feedback link.
        /// </value>
        public string FeedbackLink { get; set; } = "https://dferesearch.fra1.qualtrics.com/jfe/form/SV_6yDWcxInT3p05SZ";

        /// <summary>
        /// Gets or sets the feedback link for logged in view.
        /// </summary>
        /// <value>
        /// The logged in feedback link.
        /// </value>
        public string FeedbackLinkForLoggedInView { get; set; } = "https://dferesearch.fra1.qualtrics.com/jfe/form/SV_08qRolQiQE9ELw9";

        /// <summary>
        /// Gets or sets the contact us link.
        /// </summary>
        /// <value>
        /// The contact us link.
        /// </value>
        public string ContactUsLink { get; set; } =
            "https://customerhelpportal.education.gov.uk/";

        /// <summary>
        /// Gets or sets the http feed reader endpoint.
        /// </summary>
        /// <value>
        /// The feed reader uri.
        /// </value>
        public string FeedReaderUrl { get; set; }

        /// <summary>
        /// Gets or sets the PDF comparison URL.
        /// </summary>
        /// <value>
        /// The PDF comparison URL.
        /// </value>
        public string DocumentGeneratorPdfComparerUrl { get; set; }

        /// <summary>
        /// Gets or sets the Document generation URL.
        /// </summary>
        /// <value>
        /// The PDF generation URL.
        /// </value>
        public string DocumentGeneratorUrl { get; set; }

        /// <summary>
        /// Gets or sets the Document funding reports URL.
        /// </summary>
        /// <value>
        /// The PDF funding reports URL.
        /// </value>
        public string DocumentGeneratorFundingReportsUrl { get; set; }

        /// <summary>
        /// Gets or sets the ReRun PDF generation URL.
        /// </summary>
        /// <value>
        /// The PDF funding reports URL.
        /// </value>
        public string DocumentGeneratorRerunUrl { get; set; }

        /// <summary>
        /// Gets or sets the service now link for internal users.
        /// </summary>
        /// <value>
        /// The service now link.
        /// </value>
        public string ServiceNowLink { get; set; } =
            "https://dfe.service-now.com/serviceportal?id=sc_cat_item&sys_id=5a0e97580fd5e300a0be9acae1050eb5&sysparm_category=3b157387dbff17003b929334ca961957";

        /// <summary>
        /// Gets or sets the back to top link minimum count.
        /// </summary>
        /// <value>
        /// The back to top link minimum count.
        /// </value>
        public int BackToTopLinkMinimumCount { get; set; } = 25;

        /// <summary>
        /// Gets or sets the Funding Data Api End Point.
        /// </summary>
        /// <value>
        /// The funding Data Api End Point.
        /// </value>
        public string FundingDataApiEndPoint { get; set; }

        /// <summary>
        /// Gets or sets the logging configuration.
        /// </summary>
        public LoggingConfiguration Logging { get; set; } = new LoggingConfiguration();

        /// <summary>
        /// Gets or sets the BLOB storage.
        /// </summary>
        /// <value>
        /// The BLOB storage.
        /// </value>
        public BlobStorageConfiguration BlobStorage { get; set; } = new BlobStorageConfiguration();

        /// <summary>
        /// Gets or sets the view your funding API base address.
        /// </summary>
        /// <value>
        /// The view your funding API base address.
        /// </value>
        public string ViewYourFundingApiBaseAddress { get; set; }

        /// <summary>
        /// Gets or sets the generate spreadsheet timeout seconds.
        /// </summary>
        /// <value>
        /// The generate spreadsheet timeout seconds.
        /// </value>
        public int GenerateSpreadsheetTimeoutSeconds { get; set; } = 260;

        /// <summary>
        /// Gets or sets the funding search timeout seconds.
        /// </summary>
        /// <value>
        /// The funding search timeout seconds.
        /// </value>
        public int FundingSearchTimeoutSeconds { get; set; } = 260;

        /// <summary>
        /// Gets or sets the terminated local authority.
        /// </summary>
        /// <value>
        /// The terminated local authority.
        /// </value>
        public TerminatedLocalAuthority TerminatedLocalAuthority { get; set; }

        /// <summary>
        /// Gets or sets the recently opened local authorities.
        /// </summary>
        /// <value>
        /// The recently opened local authorities.
        /// </value>
        public RecentlyOpenedLocalAuthorities RecentlyOpenedLocalAuthorities { get; set; }

        /// <summary>
        /// Gets or sets the general login user name for tests.
        /// </summary>
        public string TestLoginUsername { get; set; }

        /// <summary>
        /// Gets or sets a login user name for a primary school for tests.
        /// </summary>
        public string TestLoginExternalUsernamePrimary { get; set; } = "10072811 - External User 51 Contracts"; // pds.sfs.test.dtest+external2@gmail.com

        /// <summary>
        /// Gets or sets a login user name for a 1619 special academy user.
        /// </summary>
        public string TestLoginExternalUsernameSpecialAcademy { get; set; } = "10004756 - External User 93 - 1619";

        /// <summary>
        /// Gets or sets a login user name for a primary mainstream school for tests.
        /// </summary>
        public string TestLoginExternalUsernamePrimaryMainStream { get; set; } = "10067283 - External User 60 Contracts";


        /// <summary>
        /// Gets or sets a login user name for a primary school for tests in year.
        /// </summary>
        public string TestLoginExternalUsernamePrimaryInYear { get; set; } = "10081419 - External User 42 Contracts"; //"pds.sfs.test.dtest+external16@gmail.com";

        /// <summary>
        /// Gets or sets a login user name for a primary school for tests in year with zero startup funding.
        /// </summary>
        public string TestLoginExternalUsernamePrimaryInYearZeroStartUpFunding { get; set; } = "10047466 - External User 77 - GAG"; // "pds.sfs.test.dtest+external19@gmail.com";

        /// <summary>
        /// Gets or sets login user name for a secondary school for tests.
        /// </summary>
        public string TestLoginExternalUsernameSecondary { get; set; } = "10038354 - External User 35 Contracts"; // "pds.sfs.test.dtest+external1@gmail.com";

        /// <summary>
        /// Gets or sets login user name for a secondary school for tests in year.
        /// </summary>
        public string TestLoginExternalUsernameSecondaryInYear { get; set; } = "10061450 - External User 44 Contracts"; // "pds.sfs.test.dtest+external17@gmail.com";

        /// <summary>
        /// Gets or sets login user name for a secondary school for tests in year with zero startup funding.
        /// </summary>
        public string TestLoginExternalUsernameSecondaryInYearZeroStartUpFunding { get; set; } = "10021072 - External User 45 Contracts"; // "pds.sfs.test.dtest+external20@gmail.com";

        /// <summary>
        /// Gets or sets login user name for an all-through school for tests.
        /// </summary>
        public string TestLoginExternalUsernameAllThrough { get; set; } = "10034949 - External User 36 Contracts"; // "pds.sfs.test.dtest+external3@gmail.com";

        /// <summary>
        /// Gets or sets login user name for an all-through school for tests in year.
        /// </summary>
        public string TestLoginExternalUsernameAllThroughInYear { get; set; } = "10047220 - External User 46 Contracts"; // "pds.sfs.test.dtest+external18@gmail.com";

        /// <summary>
        /// Gets or sets login user name for an all-through school for tests in year with zero startup funding.
        /// </summary>
        public string TestLoginExternalUsernameAllThroughInYearZeroStartUpFunding { get; set; } = "10084320 - External User 78 - GAG"; // "pds.sfs.test.dtest+external4@gmail.com";

        /// <summary>
        /// Gets or sets login user name for in-yearopener Free school tests.
        /// </summary>
        public string TestLoginExternalMainstreamInyearOpenerFreeSchool { get; set; } = "10021055 - External User 38 Contracts"; // "pds.sfs.test.dtest+external6@gmail.com";

        /// <summary>
        /// Gets or sets the test login external user for an existing special school.
        /// </summary>
        public string TestLoginExternalUserExistingSpecialSchool { get; set; } = "10063152 - External User 56 Contracts"; // "pds.sfs.test.dtest+external13@gmail.com";

        /// <summary>
        /// Gets or sets the test login external user for an existing special school.
        /// </summary>
        public string TestLoginExternalUserMat { get; set; } = "10060391 - External User 69 - Academy Trust";

        /// <summary>
        /// Gets or sets the test login external user with multiple GAG allocation history.
        /// </summary>
        public string TestLoginExternalUserMutipleGagAllocationHistory { get; set; } = "10034857 - External User34 Contracts";

        /// <summary>
        /// Gets or sets the test login external user with 1619 allocation history.
        /// </summary>
        public string TestLoginExternalUser1619 { get; set; } = "10000552 - External User 85 - 1619";

        /// <summary>
        /// Gets or sets the test login external user with 1416 allocation history.
        /// </summary>
        public string TestLoginExternalUser1416 { get; set; } = "10000552 - External User 85 - 1619";

        /// <summary>
        /// Gets or sets the test login external user with multiple 1619 allocation history.
        /// </summary>
        public string TestLoginExternalUserMutiple1619AllocationHistory { get; set; } = "10072811 - External User 51 Contracts";

        /// <summary>
        /// Gets or sets login password for tests.
        /// </summary>
        public string TestLoginPassword { get; set; }

        /// <summary>
        /// Gets or sets the connection strings.
        /// </summary>
        /// <value>
        /// The connection strings.
        /// </value>
        public ConnectionStrings ConnectionStrings { get; set; }

        /// <summary>
        /// Gets or sets the cosmos database configuration.
        /// </summary>
        /// <value>
        /// The cosmos database configuration.
        /// </value>
        public CosmosDbConfiguration CosmosDbConfiguration { get; set; }

        /// <summary>
        /// Gets or sets myesf logut url.
        /// </summary>
        public string MyesfLogoutUrl { get; set; }

        /// <summary>
        /// Gets or sets the DfE Sign In url.
        /// </summary>
        public string DfeSignInUrl { get; set; }

        /// <summary>
        /// Gets or sets MSClarityId.
        /// </summary>
        public string MSClarityId { get; set; }
    }
}