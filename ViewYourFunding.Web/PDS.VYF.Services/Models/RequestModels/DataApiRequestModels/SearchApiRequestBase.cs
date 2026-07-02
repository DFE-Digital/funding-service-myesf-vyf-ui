namespace PDS.VYF.Services.Models.RequestModels.DataApiRequestModels
{
    /// <summary>
    /// The base class for Request.
    /// </summary>
    /// <typeparam name="TApiRequestModel">The type of the API request model.</typeparam>
    public class SearchApiRequestBase<TApiRequestModel>
        where TApiRequestModel : class, new()
    {
        /// <summary>
        /// Gets or sets the funding stream periods.
        /// </summary>
        /// <value>
        /// The funding stream periods.
        /// </value>
        public List<string>? FundingStreamPeriods { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has to be latest funding.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has to be latest funding; otherwise, <c>false</c>.
        /// </value>
        public bool HasToBeLatestFunding { get; set; } = false;

        /// <summary>
        /// Gets or sets the list of ids.
        /// </summary>
        /// <value>
        /// The list of ids.
        /// </value>
        public List<string>? ListOfIds { get; set; }

        /// <summary>
        /// Gets or sets the list of ukpr ns.
        /// </summary>
        /// <value>
        /// The list of ukpr ns.
        /// </value>
        public List<string>? ListOfUKPRNs { get; set; }

        /// <summary>
        /// Gets or sets the select fields.
        /// </summary>
        /// <value>
        /// The select fields.
        /// </value>
        public HashSet<string> SelectFields { get; set; } = new HashSet<string>();

        /// <summary>
        /// Sets the select fields.
        /// </summary>
        /// <typeparam name="TSelector">The type of the selector.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// The same object.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">If params are null.</exception>
        public SearchApiRequestBase<TApiRequestModel> SetSelectFields<TSelector>(Func<TApiRequestModel, TSelector> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            this.SelectFields = typeof(TSelector)
                    .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                    .Select(a => a.Name)
                    .ToHashSet();

            return this;
        }

        /// <summary>
        /// Tries the add select fields.
        /// </summary>
        /// <typeparam name="TSelector">The type of the selector.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns>Always true.</returns>
        /// <exception cref="ArgumentNullException">selector should not be null.</exception>
        public bool TryAddSelectFields<TSelector>(Func<TApiRequestModel, TSelector> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            this.SelectFields ??= new HashSet<string>();

            var newSelectFields = typeof(TSelector)
                    .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                    .Where(a => !this.SelectFields.Contains(a.Name))
                    .Select(a => a.Name);

            foreach (var item in newSelectFields)
            {
                this.SelectFields.Add(item);
            }

            return true;
        }

        /// <summary>
        /// Sets the select fields except.
        /// </summary>
        /// <typeparam name="TSelector">The type of the selector.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns>Same object.</returns>
        public SearchApiRequestBase<TApiRequestModel> SetSelectFieldsExcept<TSelector>(Func<TApiRequestModel, TSelector> selector)
        {
            var excludedFields = typeof(TSelector)
                    .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                    .Select(a => a.Name)
                    .ToHashSet();

            this.SelectFields = typeof(TApiRequestModel)
                    .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                    .Where(prop => !excludedFields.Contains(prop.Name))
                    .Select(a => a.Name)
                    .ToHashSet();

            return this;
        }
    }
}
