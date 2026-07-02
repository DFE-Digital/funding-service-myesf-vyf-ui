namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// Class containing information about the action to take based on the selection made on the 'Select a funding type' page.
    /// </summary>
    public class WhichAllocationOptionAction
    {
        /// <summary>
        /// Gets or sets the route name to direct to.
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// Gets or sets the route values to pass to the route.
        /// </summary>
        public object RouteValues { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether or not to pass the route values when scripts are enabled. When scripts are disabled, the route values will always be passed.
        /// </summary>
        public bool UseRouteValuesWhenScriptsEnabled { get; set; }
    }
}
