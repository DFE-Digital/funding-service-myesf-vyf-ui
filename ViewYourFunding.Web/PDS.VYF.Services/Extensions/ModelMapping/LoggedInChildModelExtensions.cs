namespace PDS.VYF.Services.Extensions.ModelMapping
{
    using Newtonsoft.Json;
    using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
    using PDS.ViewYourFunding.Services.Interfaces.Models;
    using PDS.ViewYourFunding.Services.Models;
    using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

    /// <summary>
    /// Extensions class for LoggedInChildModel.
    /// </summary>
    public static class LoggedInChildModelExtensions
    {
        /// <summary>
        /// Gets the ProviderFundingApiSearchResponse for the given LoggedInChildModels.
        /// </summary>
        /// <param name="loggedInChildModels">The LoggedInChildModels.</param>
        /// <returns>The ProviderFundingApiSearchResponse.</returns>
        public static ProviderFundingApiSearchResponse GetProviderFundingApiSearchResponse(params LoggedInChildModel[] loggedInChildModels)
        {
            ProviderFundingApiSearchResponse providerFundingApiSearchResponse = new();

            providerFundingApiSearchResponse.ProviderFunding = loggedInChildModels.Select(model => model.GetFundingApiSearchProviderFunding());

            return providerFundingApiSearchResponse;
        }

        public static IFundingApiSearchResponseProviderFunding? Combine(this IFundingApiSearchResponseProviderFunding? responseA, IFundingApiSearchResponseProviderFunding? responseB)
        {
            if (responseA != null && responseB != null)
            {
                return new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = responseA.ProviderFunding.Concat(responseB.ProviderFunding),
                };
            }
            else if (responseA == null && responseB != null)
            {
                return responseB;
            }
            else if (responseA != null && responseB == null)
            {
                return responseA;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the FundingApiSearchProviderFunding for the given LoggedInChildModel.
        /// </summary>
        /// <param name="loggedInChildModel">The LoggedInChildModel.</param>
        /// <returns>The FundingApiSearchProviderFunding.</returns>
        public static FundingApiSearchProviderFunding GetFundingApiSearchProviderFunding(this LoggedInChildModel loggedInChildModel) => new FundingApiSearchProviderFunding()
        {
            Id = loggedInChildModel.Id,
            ParentId = loggedInChildModel.ParentInfo?.FirstOrDefault()?.ParentId,
            FundingPeriodCode = loggedInChildModel.FundingPeriodCode,
            FundingStreamCode = loggedInChildModel.FundingStreamCode,
            StatusChangedDate = loggedInChildModel.StatusChangedDate ?? DateTime.Today,
            VariationReason = loggedInChildModel.VariationReasons == null ? string.Empty : string.Join(", ", loggedInChildModel.VariationReasons),
            FundingVersion = loggedInChildModel.FundingVersion,
            IsFirstStatementChannelVersion = loggedInChildModel.StatementType == "New",
            SchemaVersion = loggedInChildModel.SchemaVersion,
            TemplateVersion = loggedInChildModel.TemplateVersion,
            OrganisationName = loggedInChildModel.OrganisationName,
            SearchableOrganisationName = loggedInChildModel.SearchableOrganisationName,
            OrganisationUkprn = loggedInChildModel.OrganisationUkprn,
            OrganisationDfeNumber = loggedInChildModel.OrganisationDfeNumber,
            OrganisationTown = loggedInChildModel.OrganisationTown,
            OrganisationPostcode = loggedInChildModel.OrganisationPostcode,
            ParentName = loggedInChildModel.ParentInfo?.FirstOrDefault()?.ParentName,
            ParentPrimaryIdentifier = loggedInChildModel.ParentInfo?.FirstOrDefault()?.ParentPrimaryIdentifier,
            ParentProviderType = loggedInChildModel.ParentInfo?.FirstOrDefault()?.ParentProviderType,
            ProviderType = loggedInChildModel.ProviderType,
            ProviderSubType = loggedInChildModel.ProviderSubType,
            ProviderStatus = loggedInChildModel.ProviderStatus,
            CloseReason = loggedInChildModel.CloseReason,
            OpenReason = loggedInChildModel.OpenReason,
            TotalAmount = loggedInChildModel.TotalAmount ?? 0,
            FundingValue = GetFundingValueString(
                                            loggedInChildModel.TotalAmount ?? 0,
                                            loggedInChildModel.FundingLines ?? loggedInChildModel.FundingLinesForSummary ?? new List<LoggedInTemplateLine>(),
                                            loggedInChildModel.Calculations ?? loggedInChildModel.CalculationsForSummary ?? new List<LoggedInCalculation>()),
            GroupingReason = loggedInChildModel.ParentInfo?.FirstOrDefault()?.GroupingReason,
            PhaseOfEducation = loggedInChildModel.PhaseOfEducation,
            DateOpened = loggedInChildModel.DateOpened,
            DateClosed = loggedInChildModel.DateClosed,
            ProviderUpin = loggedInChildModel.ProviderUrn,
            ProviderUrn = loggedInChildModel.ProviderUrn,
            LocalAuthorityName = loggedInChildModel.LocalAuthorityName,
            ParliamentaryConstituencyName = loggedInChildModel.ParliamentaryConstituencyName,
            ParliamentaryConstituencyCode = loggedInChildModel.ParliamentaryConstituencyCode,
            StatementChannelVersion = loggedInChildModel.StatementChannelVersion,
        };

        /// <summary>
        /// Gets the FundingValue string for the given totalValue, templateLines, and calculations.
        /// </summary>
        /// <param name="totalValue">The total value.</param>
        /// <param name="templateLines">The template lines.</param>
        /// <param name="calculations">The calculations.</param>
        /// <returns>The FundingValue string.</returns>
        private static string GetFundingValueString(double totalValue, IEnumerable<LoggedInTemplateLine> templateLines, IEnumerable<LoggedInCalculation> calculations)
        {
            FundingValueNested_1_2 result = new()
            {
                TotalValue = totalValue,
                FundingLines = templateLines
                                .Select(a => new FundingLineNoNesting()
                                {
                                    TemplateLineId = a.TemplateLineId,
                                    Value = a.Value,
                                    Type = a.Type,
                                    DistributionPeriods = a.DistributionPeriods?.Select(b => new DistributionPeriod()
                                    {
                                        DistributionPeriodId = b.DistributionPeriodId,
                                        Value = b.Value ?? 0,
                                    })?.ToList(),
                                }).ToList(),
                Calculations = calculations.Select(a => new CalculationNoNesting() { TemplateCalculationId = a.TemplateCalculationId, Value = a.Value }).ToList(),
            };

            return JsonConvert.SerializeObject(result);
        }
    }
}
