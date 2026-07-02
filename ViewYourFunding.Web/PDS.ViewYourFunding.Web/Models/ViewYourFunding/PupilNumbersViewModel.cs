namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The pupil numbers view model.
    /// </summary>
    public class PupilNumbersViewModel
    {
        /// <summary>
        /// Gets or sets number of pupils.
        /// </summary>
        public decimal PupilNumber { get; set; }

        /// <summary>
        /// Gets or sets number of pupils display text e.g. "Number of Pupils".
        /// </summary>
        public string PupilNumberDisplayText { get; set; }

        /// <summary>
        /// Gets a value indicating whether determines if pupil numbers link is to be shown. If whole number then do not show link.
        /// </summary>
        public bool ShowGovLink => PupilNumber % 1 != 0;
    }
}