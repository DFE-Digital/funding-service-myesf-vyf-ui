using PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Services.Tests.Integration.Models
{
    /// <summary>
    /// The Test Cosmos Document class.
    /// </summary>
    /// <seealso cref="CosmosDocument" />
    public class TestCosmosDocument : CosmosDocument
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>
        /// The name of the collection.
        /// </value>
        public override string CollectionName => "fundingUiIntegrationTests";
    }
}