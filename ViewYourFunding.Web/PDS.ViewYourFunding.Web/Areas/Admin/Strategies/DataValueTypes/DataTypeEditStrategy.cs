using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.DataValueTypes
{
    /// <summary>
    /// The publication strategy for handling related publication actions.
    /// </summary>
    public class DataTypeEditStrategy
    {
        /// <summary>
        /// Gets or sets the data type edits.
        /// </summary>
        /// <value>
        /// The data type edits.
        /// </value>
        public IList<IDataTypeEdit> DataTypeEdits { get; set; }
    }
}