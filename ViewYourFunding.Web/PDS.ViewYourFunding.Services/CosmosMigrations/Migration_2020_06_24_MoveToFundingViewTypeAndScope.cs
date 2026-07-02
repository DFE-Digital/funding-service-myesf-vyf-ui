using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.CosmosMigrations
{
    /// <summary>
    /// Migrate to new field properties.
    /// </summary>
    public static class Migration_2020_06_24_MoveToFundingViewTypeAndScope
    {
        /// <summary>
        /// Migrate to new field properties.
        /// </summary>
        /// <param name="cosmosDatabaseService">The cosmos db service to run against.</param>
        /// <param name="logger">The logger to use in case of error.</param>
        /// <returns>An awaitable task.</returns>
        public static async Task Run(
            ICosmosDbService<LayoutModel> cosmosDatabaseService,
            ILoggerAdapter<LayoutManagementService> logger)
        {
            var sql =
                @"SELECT *
                FROM c
                where (IS_DEFINED(c.FundingViewType) = false or c.FundingViewType = null)
                and (IS_DEFINED(c.LayoutType) = true and c.LayoutType != null and c.LayoutType != '')";

            try
            {
                var currentList = await cosmosDatabaseService.RunQueryAsync<Dictionary<string, object>>(sql);

                foreach (var currentItem in currentList)
                {
                    var id = (string)currentItem["id"];
                    var type = (string)currentItem["LayoutType"];

                    var updatedItem = new LayoutModel
                    {
                        Id = id,
                        CreatedDate = (DateTime)currentItem["CreatedDate"],
                        DeletedDateTime = currentItem.ContainsKey("DeletedDateTime") ? (DateTime?)currentItem["DeletedDateTime"] : null,
                        FundingStreamId = (int)(long)currentItem["FundingStreamId"],
                        LastModifiedBy = (string)currentItem["LastModifiedBy"],
                        LastModifiedDateTime = (DateTime)currentItem["LastModifiedDateTime"],
                        LayoutJsonData = (string)currentItem["LayoutJsonData"],
                        LayoutName = (string)currentItem["LayoutName"]
                    };

                    switch (type)
                    {
                        case "LocalAuthoritySummary":
                            updatedItem.FundingViewType = FundingViewType.ViewData.ToString();
                            updatedItem.FundingViewScope = FundingViewScope.OrganisationSummary.ToString();

                            break;
                        case "LocalAuthorityFundingBreakDown":
                            updatedItem.FundingViewType = FundingViewType.ViewData.ToString();
                            updatedItem.FundingViewScope = FundingViewScope.Organisation.ToString();

                            break;
                        case "LocalAuthorityHistory":
                            updatedItem.FundingViewType = FundingViewType.ViewData.ToString();
                            updatedItem.FundingViewScope = FundingViewScope.OrganisationHistory.ToString();

                            break;
                        case "ProviderSummary":
                            updatedItem.FundingViewType = FundingViewType.ViewData.ToString();
                            updatedItem.FundingViewScope = FundingViewScope.ProviderSummary.ToString();

                            break;
                        case "ProviderFundingBreakDown":
                            updatedItem.FundingViewType = FundingViewType.ViewData.ToString();
                            updatedItem.FundingViewScope = FundingViewScope.Provider.ToString();

                            break;
                        case "ProviderHistory":
                            updatedItem.FundingViewType = FundingViewType.ViewData.ToString();
                            updatedItem.FundingViewScope = FundingViewScope.ProviderHistory.ToString();

                            break;
                        case "LocalAuthoritySpreadsheet":
                            updatedItem.FundingViewType = FundingViewType.Spreadsheet.ToString();
                            updatedItem.FundingViewScope = FundingViewScope.Organisation.ToString();

                            break;
                        case "ProviderSpreadsheet":
                            updatedItem.FundingViewType = FundingViewType.Spreadsheet.ToString();
                            updatedItem.FundingViewScope = FundingViewScope.Provider.ToString();

                            break;
                        case "NationalSpreadsheet":
                            updatedItem.FundingViewType = FundingViewType.Spreadsheet.ToString();
                            updatedItem.FundingViewScope = FundingViewScope.National.ToString();

                            break;
                        case "National":
                            updatedItem.FundingViewType = FundingViewType.ViewData.ToString();
                            updatedItem.FundingViewScope = FundingViewScope.National.ToString();

                            break;
                    }

                    await cosmosDatabaseService.UpdateAsync(id, updatedItem);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.Message, ex);
            }
        }
    }
}