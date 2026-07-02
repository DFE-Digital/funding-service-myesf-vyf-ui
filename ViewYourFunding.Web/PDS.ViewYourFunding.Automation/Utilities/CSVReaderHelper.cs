using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ViewYourFunding.Automation.Utilities
{
    /// <summary>
    /// The csv helper.
    /// </summary>
    public static class CSVReaderHelper
    {
        /// <summary>
        /// Gets the records from the file downloaded in the test.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The list of records in the file.</returns>
        public static IEnumerable<dynamic> GetActualRecords(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<dynamic>().ToList();
            }
        }

        /// <summary>
        /// Gets the records from the expected file.
        /// </summary>
        /// <param name="fileName">The expected file name.</param>
        /// <returns>The list of records in the file.</returns>
        public static List<dynamic> GetTestRecords(string fileName)
        {
            using (var reader = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream($"PDS.ViewYourFunding.Automation.CSVResources.{fileName}"))
            using (var csv = new CsvReader(new StreamReader(reader), CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<dynamic>().ToList();
            }
        }
    }
}
