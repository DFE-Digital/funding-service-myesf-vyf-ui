using Newtonsoft.Json;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Implementations
{
    /// <summary>
    /// The EF Core Db funding stream repository.
    /// </summary>
    /// <seealso cref="INextPaymentRepository" />
    public class FundingStreamRepository : Repository<FundingStream>, IFundingStreamRepository
    {
        private readonly ILoggerAdapter<Repository<FundingStream>> _loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingStreamRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="loggerService">The logger service.</param>
        public FundingStreamRepository(
            Context dbContext,
            ILoggerAdapter<Repository<FundingStream>> loggerService)
            : base(dbContext, loggerService)
        {
            _loggerService = loggerService;
        }

        /// <inheritdoc/>
        public async Task<FundingStream> CreateFundingStream(FundingStream fundingStream)
        {
            fundingStream.LastUpdatedAt = DateTime.Now;
            fundingStream.CreatedAt = DateTime.Now;
            var createResult = await AddAsync(fundingStream);
            _loggerService?.LogInformation(Message(fundingStream, createResult, nameof(CreateFundingStream)));

            return createResult;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteFundingStream(FundingStream fundingStream)
        {
            var changes = new Dictionary<string, string>();
            var dbFundingStream = await GetDatabaseFundingStream(fundingStream);
            if (dbFundingStream == null)
            {
                return false;
            }

            dbFundingStream.DeletedAt = DateTime.Now;
            var saveAffectedRecords = await SaveChangesAsync();

            var result = saveAffectedRecords > 0;

            if (result)
            {
                _loggerService?.LogInformation(
                    $" Funding stream {dbFundingStream.Id} ({dbFundingStream.FundingStreamCode ?? "?"}) updated by {dbFundingStream.LastUpdatedBy} for the following properties {string.Join(",", changes.Select(x => $"{x.Key} => {x.Value}"))}");
            }
            else
            {
                _loggerService?.LogInformation(
                    $" Funding stream {dbFundingStream.Id} ({dbFundingStream.FundingStreamCode ?? "?"}) didn't update");
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FundingStream>> GetAllFundingStreams(bool activeOnly, params FetchData[] fetchData)
        {
            var fundingStreams = activeOnly ?
                await GetAllAsync(fundingStream => fundingStream.DeletedAt == null && fundingStream.Active == true) :
                await GetAllAsync(fundingStream => fundingStream.DeletedAt == null);

            var fundingStreamIds = fundingStreams.Select(fs => fs.Id).ToArray();

            var getPublications = fetchData?.Contains(FetchData.Publications) == true || fetchData?.Contains(FetchData.Publications_PublicationLayouts) == true;
            var getSettingValues = fetchData?.Contains(FetchData.SettingValues_Setting) == true;
            var getNextPayments = fetchData?.Contains(FetchData.NextPayments_NextPaymentType) == true;
            var getNextPaymentTypes = fetchData?.Contains(FetchData.NextPaymentTypes_NextPayments) == true;

            if (getPublications)
            {
                var getPublicationLayouts = fetchData?.Contains(FetchData.Publications_PublicationLayouts) == true;

                var publications = await GetAllAsyncType<Publication>(
                    publication => fundingStreamIds.Contains(publication.FundingStreamId),
                    includeProperties: getPublicationLayouts ? "PublicationLayouts" : null);

                // Assign funding streams to publications
                publications.Select(p => p.FundingStream = fundingStreams.First(fs => fs.Id == p.FundingStreamId)).ToList();

                // Assign publications to funding streams
                fundingStreams.Select(fs => fs.Publications = publications.Where(p => p.FundingStreamId == fs.Id).ToList()).ToList();
            }

            if (getSettingValues)
            {
                var settingValues = await GetAllAsyncType<SettingValue>(
                    settingValue => fundingStreamIds.Contains(settingValue.FundingStreamId),
                    includeProperties: "Setting");

                // Assign funding streams to setting values
                settingValues.Select(sv => sv.FundingStream = fundingStreams.FirstOrDefault(fs => fs.Id == sv.FundingStreamId)).ToList();

                // Assign setting values to funding streams
                fundingStreams.Select(fs => fs.SettingValues = settingValues.Where(sv => sv.FundingStreamId == fs.Id).ToList()).ToList();
            }

            if (getNextPayments)
            {
                var nextPayments = await GetAllAsyncType<NextPayment>(
                    nextPayment => fundingStreamIds.Contains(nextPayment.FundingStreamId),
                    includeProperties: "NextPaymentType");

                // Assign funding streams to next payments
                nextPayments.Select(np => np.FundingStream = fundingStreams.FirstOrDefault(fs => fs.Id == np.FundingStreamId)).ToList();

                // Assign next paymments to funding streams
                fundingStreams.Select(fs => fs.NextPayments = nextPayments.Where(np => np.FundingStreamId == fs.Id).ToList()).ToList();
            }

            if (getNextPaymentTypes)
            {
                var nextPaymentTypes = await GetAllAsyncType<NextPaymentType>(
                    nextPaymentType => fundingStreamIds.Contains(nextPaymentType.FundingStreamId),
                    includeProperties: "NextPayments");

                // Assign funding streams to next payment types
                nextPaymentTypes.Select(npt => npt.FundingStream = fundingStreams.FirstOrDefault(fs => fs.Id == npt.FundingStreamId)).ToList();

                // Assign next payment types to funding streams
                fundingStreams.Select(fs => fs.NextPaymentTypes = nextPaymentTypes.Where(npt => npt.FundingStreamId == fs.Id).ToList()).ToList();
            }

            return fundingStreams;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateFundingStream(FundingStream fundingStream)
        {
            var changes = new Dictionary<string, string>();
            var dbFundingStream = await GetDatabaseFundingStream(fundingStream);
            if (dbFundingStream == null)
            {
                return false;
            }

            GetChanges(fundingStream, dbFundingStream, changes);
            dbFundingStream.LastUpdatedAt = DateTime.Now;

            var saveAffectedRecords = await SaveChangesAsync();

            var result = saveAffectedRecords > 0;
            if (result)
            {
                _loggerService?.LogInformation(
                    $" Funding stream {dbFundingStream.Id} ({dbFundingStream.FundingStreamCode ?? "?"}) updated by {dbFundingStream.LastUpdatedBy} for the following properties {string.Join(",", changes.Select(x => $"{x.Key} => {x.Value}"))}");
            }
            else
            {
                _loggerService?.LogInformation(
                    $" Funding stream {dbFundingStream.Id} ({dbFundingStream.FundingStreamCode ?? "?"}) didn't update");
            }

            return result;
        }

        private static string Message(FundingStream fundingStream, FundingStream actionResult, string actionName)
        {
            return $"Funding Stream update {actionName} for {GetFundingStreamData(fundingStream)} had result {GetFundingStreamData(actionResult)}";
        }

        private static void GetChanges(FundingStream fundingStream, FundingStream dbFundingStream, Dictionary<string, string> changes)
        {
            if (dbFundingStream.FundingStreamCode != fundingStream.FundingStreamCode)
            {
                changes.Add(nameof(dbFundingStream.FundingStreamCode), $"{dbFundingStream.FundingStreamCode} => {fundingStream.FundingStreamCode}");
                dbFundingStream.FundingStreamCode = fundingStream.FundingStreamCode;
            }

            if (dbFundingStream.FundingStreamName != fundingStream.FundingStreamName)
            {
                changes.Add(nameof(dbFundingStream.FundingStreamName), $"{dbFundingStream.FundingStreamName} => {fundingStream.FundingStreamName}");
                dbFundingStream.FundingStreamName = fundingStream.FundingStreamName;
            }

            if (dbFundingStream.FundingStreamNameWithinSentence != fundingStream.FundingStreamNameWithinSentence)
            {
                changes.Add(nameof(dbFundingStream.FundingStreamNameWithinSentence), $"{dbFundingStream.FundingStreamNameWithinSentence} => {fundingStream.FundingStreamNameWithinSentence}");
                dbFundingStream.FundingStreamNameWithinSentence = fundingStream.FundingStreamNameWithinSentence;
            }

            if (dbFundingStream.FundingStreamBusinessAllocationName != fundingStream.FundingStreamBusinessAllocationName)
            {
                changes.Add(nameof(dbFundingStream.FundingStreamBusinessAllocationName), $"{dbFundingStream.FundingStreamBusinessAllocationName} => {fundingStream.FundingStreamBusinessAllocationName}");
                dbFundingStream.FundingStreamBusinessAllocationName = fundingStream.FundingStreamBusinessAllocationName;
            }

            if (dbFundingStream.FundingStreamCodePubliclyKnown != fundingStream.FundingStreamCodePubliclyKnown)
            {
                changes.Add(nameof(dbFundingStream.FundingStreamCodePubliclyKnown), $"{dbFundingStream.FundingStreamCodePubliclyKnown} => {fundingStream.FundingStreamCodePubliclyKnown}");
                dbFundingStream.FundingStreamCodePubliclyKnown = fundingStream.FundingStreamCodePubliclyKnown;
            }

            if (dbFundingStream.RelevantForOrganisations_LoggedIn != fundingStream.RelevantForOrganisations_LoggedIn)
            {
                changes.Add(nameof(dbFundingStream.RelevantForOrganisations_LoggedIn), $"{dbFundingStream.RelevantForOrganisations_LoggedIn} => {fundingStream.RelevantForOrganisations_LoggedIn}");
                dbFundingStream.RelevantForOrganisations_LoggedIn = fundingStream.RelevantForOrganisations_LoggedIn;
            }

            if (dbFundingStream.RelevantForOrganisations_Public != fundingStream.RelevantForOrganisations_Public)
            {
                changes.Add(nameof(dbFundingStream.RelevantForOrganisations_Public), $"{dbFundingStream.RelevantForOrganisations_Public} => {fundingStream.RelevantForOrganisations_Public}");
                dbFundingStream.RelevantForOrganisations_Public = fundingStream.RelevantForOrganisations_Public;
            }

            if (dbFundingStream.RelevantForNational != fundingStream.RelevantForNational)
            {
                changes.Add(nameof(dbFundingStream.RelevantForNational), $"{dbFundingStream.RelevantForNational} => {fundingStream.RelevantForNational}");
                dbFundingStream.RelevantForNational = fundingStream.RelevantForNational;
            }

            if (dbFundingStream.RelevantForProviders_LoggedIn != fundingStream.RelevantForProviders_LoggedIn)
            {
                changes.Add(nameof(dbFundingStream.RelevantForProviders_LoggedIn), $"{dbFundingStream.RelevantForProviders_LoggedIn} => {fundingStream.RelevantForProviders_LoggedIn}");
                dbFundingStream.RelevantForProviders_LoggedIn = fundingStream.RelevantForProviders_LoggedIn;
            }

            if (dbFundingStream.RelevantForProviders_Public != fundingStream.RelevantForProviders_Public)
            {
                changes.Add(nameof(dbFundingStream.RelevantForProviders_Public), $"{dbFundingStream.RelevantForProviders_Public} => {fundingStream.RelevantForProviders_Public}");
                dbFundingStream.RelevantForProviders_Public = fundingStream.RelevantForProviders_Public;
            }

            if (dbFundingStream.HistoryIndependentOfPublications != fundingStream.HistoryIndependentOfPublications)
            {
                changes.Add(nameof(dbFundingStream.HistoryIndependentOfPublications), $"{dbFundingStream.HistoryIndependentOfPublications} => {fundingStream.HistoryIndependentOfPublications}");
                dbFundingStream.HistoryIndependentOfPublications = fundingStream.HistoryIndependentOfPublications;
            }

            if (dbFundingStream.Active != fundingStream.Active)
            {
                changes.Add(nameof(dbFundingStream.Active), $"{dbFundingStream.Active} => {fundingStream.Active}");
                dbFundingStream.Active = fundingStream.Active;
            }
        }

        private static string GetFundingStreamData(FundingStream fundingStream)
        {
            if (fundingStream == null)
            {
                return "null";
            }

            return JsonConvert.SerializeObject(
                fundingStream,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        private async Task<FundingStream> GetDatabaseFundingStream(FundingStream fundingStream)
        {
            var dbFundingStream = await GetAsync(fundingStream.Id);
            return dbFundingStream;
        }
    }
}