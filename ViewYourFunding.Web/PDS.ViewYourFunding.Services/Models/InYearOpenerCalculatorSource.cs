namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// The In Year Opener Calculator source model.
    /// </summary>
    public class InYearOpenerCalculatorSource
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is academic year in year opener.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is academic year in year opener; otherwise, <c>false</c>.
        /// </value>
        public bool IsAcademicYearInYearOpener { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is previous year post april opener.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is previous year post april opener; otherwise, <c>false</c>.
        /// </value>
        public bool IsPreviousYearPostAprilOpener { get; set; }

        /// <summary>
        /// Gets or sets the opening reason.
        /// </summary>
        /// <value>
        /// The opening reason.
        /// </value>
        public string OpeningReason { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is open days equal to full year days.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open days equal to full year days; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpenDaysEqualToFullYearDays { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if the selected statement has "closed reason" as "Fresh Start" and has value in "date closed" field.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance "closed reason" as "Fresh Start" and has value in "date closed" field; otherwise, <c>false</c>.
        /// </value>
        public bool ReBrokerageCloseReason { get; set; }
    }
}