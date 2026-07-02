using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.DataValueTypes
{
    /// <summary>
    /// The Data Type Edit Strategy Factory.
    /// </summary>
    public static class DataTypeEditStrategyFactory
    {
        /// <summary>
        /// Gets the data type edit strategy.
        /// </summary>
        /// <returns>The DataTypeEditStrategy.</returns>
        public static DataTypeEditStrategy GetDataTypeEditStrategy()
        {
            return new DataTypeEditStrategy
            {
                DataTypeEdits = new List<IDataTypeEdit>
                {
                   new BoolDataTypeEdit(),
                   new DateDataTypeEdit(),
                   new DateTimeDataTypeEdit(),
                   new StringDataTypeEdit(),
                   new IntDataTypeEdit(),
                   new TimeDataTypeEdit()
                }
            };
        }
    }
}