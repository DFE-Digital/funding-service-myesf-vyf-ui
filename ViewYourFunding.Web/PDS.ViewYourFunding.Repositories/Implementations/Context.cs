using Microsoft.EntityFrameworkCore;
using PDS.ViewYourFunding.Repositories.DataModels;
using FundingStream = PDS.ViewYourFunding.Repositories.DataModels.FundingStream;
using Publication = PDS.ViewYourFunding.Repositories.DataModels.Publication;
using SettingValue = PDS.ViewYourFunding.Repositories.DataModels.SettingValue;

namespace PDS.ViewYourFunding.Repositories.Implementations
{
    /// <summary>
    /// The Vyf Context.
    /// </summary>
    /// <seealso cref="DbContext" />
    public class Context : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        /// <param name="options">The DB options.</param>
        public Context(DbContextOptions options)
        : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the funding streams.
        /// </summary>
        public DbSet<FundingStream> FundingStreams { get; set; }

        /// <summary>
        /// Gets or sets the publications.
        /// </summary>
        public DbSet<Publication> Publications { get; set; }

        /// <summary>
        /// Gets or sets the publication layouts.
        /// </summary>
        public DbSet<PublicationLayout> PublicationLayouts { get; set; }

        /// <summary>
        /// Gets or sets the setting values.
        /// </summary>
        /// <value>
        /// The setting values.
        /// </value>
        public DbSet<SettingValue> SettingValues { get; set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public DbSet<Setting> Settings { get; set; }

        /// <summary>
        /// Gets or sets the next payments.
        /// </summary>
        /// <value>
        /// The next payment.
        /// </value>
        public DbSet<NextPayment> NextPayments { get; set; }

        /// <summary>
        /// Gets or sets the next payment type.
        /// </summary>
        /// <value>
        /// The next payment type.
        /// </value>
        public DbSet<NextPaymentType> NextPaymentTypes { get; set; }

        /// <summary>
        /// Gets or sets the global settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public DbSet<GlobalSetting> GlobalSettings { get; set; }
    }
}