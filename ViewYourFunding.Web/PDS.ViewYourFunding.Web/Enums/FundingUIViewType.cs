namespace PDS.ViewYourFunding.Web.Enums
{
    /// <summary>
    /// Relevant for types for funding.
    /// </summary>
    public enum FundingUIViewType
    {
        /// <summary>
        /// Relevant for providers who are logged in
        /// </summary>
        Providers_LoggedIn,

        /// <summary>
        /// Relevant for providers on the public view.
        /// </summary>
        Providers_Public,

        /// <summary>
        /// Relevant for organisations who are logged in.
        /// </summary>
        Organisations_LoggedIn,

        /// <summary>
        /// Relevant for organisations on the public view.
        /// </summary>
        Organisations_Public,

        /// <summary>
        /// Relevant fo the nation view.
        /// </summary>
        National
    }
}