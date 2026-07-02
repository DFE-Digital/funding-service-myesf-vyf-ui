using Newtonsoft.Json;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Schema version helper.
    /// </summary>
    public static class SchemaVersionHelper
    {
        /// <summary>
        /// Gets the data validated schema version.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The schema version.</returns>
        public static double GetFundingValueDataValidatedSchemaVersion(this IFundingApiSearch data)
        {
            double.TryParse(data.SchemaVersion, out var schemaVersion);
            switch (schemaVersion)
            {
                case 1.1:
                    try
                    {
                        JsonConvert.DeserializeObject<FundingValueNested_1_1>(data.FundingValue);

                        return schemaVersion;
                    }
                    catch
                    {
                        JsonConvert.DeserializeObject<FundingValueNested_1_2>(data.FundingValue);

                        return 1.2;
                    }

                case 1.2:
                    try
                    {
                        JsonConvert.DeserializeObject<FundingValueNested_1_2>(data.FundingValue);

                        return schemaVersion;
                    }
                    catch
                    {
                        JsonConvert.DeserializeObject<FundingValueNested_1_1>(data.FundingValue);

                        return 1.1;
                    }

                case 1.0:
                    try
                    {
                        JsonConvert.DeserializeObject<FundingValueNested_1_0>(data.FundingValue);

                        return schemaVersion;
                    }
                    catch
                    {
                        JsonConvert.DeserializeObject<FundingValueNested_1_1>(data.FundingValue);

                        return 1.1;
                    }

                default:
                    return schemaVersion;
            }
        }
    }
}