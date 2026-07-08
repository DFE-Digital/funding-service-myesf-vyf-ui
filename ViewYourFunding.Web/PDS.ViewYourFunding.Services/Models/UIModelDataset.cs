using PDS.ViewYourFunding.Services.Helper;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A dataset (for example, a local authrority).
    /// </summary>
    public class UiModelDataset
    {
        /// <summary>
        /// Gets or sets an id to refer to the dataset with.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the grouping type (e.g. LocalAuthority).
        /// </summary>
        public string GroupingType { get; set; }

        /// <summary>
        /// Gets or sets the parent grouping type (e.g. LocalAuthority).
        /// </summary>
        public string ParentGroupingType { get; set; }

        /// <summary>
        /// Gets or sets the property to order by (optional).
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Gets or sets the property to order by, with the ordering being descending (optional).
        /// </summary>
        public string OrderByDescending { get; set; }

        /// <summary>
        /// Gets or sets rows to prepend to the start of the dataset data.
        /// </summary>
        public List<UiModelFundingApiSearchFunding> PrependRows { get; set; }

        /// <summary>
        /// Gets or sets rows to apepend to the end of the dataset data.
        /// </summary>
        public List<UiModelFundingApiSearchFunding> AppendRows { get; set; }

        /// <summary>
        /// Gets or sets the number of rows to limit to in the dataset (optional).
        /// </summary>
        public int? Take { get; set; }

        /// <summary>
        /// Gets or sets a where clause, in the format NAME=VALUE, NAME!+VALUE, NAME=VALUE|NAME=VALUE or NAME=VALUEAndName!=VALUE or some combination (optional).
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets a where clause, in the format NAME=VALUE, NAME!+VALUE, NAME=VALUE|NAME=VALUE or NAME=VALUEAndName!=VALUE or some combination (optional).
        /// </summary>
        public List<UiModelDataSetFilter> Expressions { get; set; }

        /// <summary>
        /// Gets or sets a Expressions Separator, in the format AND or OR.
        /// </summary>
        public string ExpressionSeparator { get; set; }

        /// <summary>
        /// Gets or sets the name of the dataset. 'funding' or 'providerfunding'.
        /// </summary>
        public string DatasetName { get; set; }

        /// <summary>
        /// Gets or sets the funding stream to limit to (optional).
        /// </summary>
        public string[] FundingStreamMappings { get; set; }

        /// <summary>
        /// Gets or sets the limited to expression (optional). This property is passed to the Data API to act as a filter.
        /// Usage is 'DatasetX_First_Y' where X is the dataset position (1 indexed) is something
        /// and Y is 'ID', 'LaName' and 'GroupName'.
        /// _
        /// _
        /// Examples are; 'Dataset1_First_ID', 'Dataset2_First_LaName' and 'Dataset1_First_GroupName'.
        /// </summary>
        public string LimitedTo { get; set; }

        /// <summary>
        /// Gets or sets the then by.
        /// </summary>
        /// <value>
        /// The then by.
        /// </value>
        public string ThenBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to group by the organisation ukprn.
        /// </summary>
        public bool GroupByOrganisationUkprn { get; set; }

        /// <summary>
        /// Gets or sets the value indicating position of dataset on layout.
        /// </summary>
        public int Position { get; set; }
    }
}