using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Constants
{
    /// <summary>
    /// Constants for funding document file format extensions.
    /// </summary>
    public static class FundingDocumentFileType
    {
        /// <summary>
        /// A spreadsheet CSV format (as used by open office).
        /// </summary>
        public const string Spreadsheet_CSVFormat = "csv";

        /// <summary>
        /// A spreadsheet CSV format content type.
        /// </summary>
        public const string Spreadsheet_CSVFormatContentType = "text/csv; charset=utf-8";

        /// <summary>
        /// A spreadsheet open format (as used by open office).
        /// </summary>
        public const string Spreadsheet_OpenFormat = "ods";

        /// <summary>
        /// A spreadsheet open format (as used by open office) content type.
        /// </summary>
        public const string Spreadsheet_OpenFormatContentType = "application/vnd.oasis.opendocument.spreadsheet; charset=utf-8";

        /// <summary>
        /// A spreadsheet that can be opened in Microsoft Excel.
        /// </summary>
        public const string Spreadsheet_ExcelFormat = "xls";

        /// <summary>
        /// A spreadsheet that can be opened in Microsoft Excel content type.
        /// </summary>
        public const string Spreadsheet_ExcelFormatContentType = "application/vnd.ms-excel; charset=utf-8";

        /// <summary>
        /// The layout file json content type.
        /// </summary>
        public const string LayoutFile_JsonContentType = "application/json; charset=utf-8";

        /// <summary>
        /// Gets all file formats.
        /// </summary>
        public static Dictionary<string, string> FileFormats
        {
            get => new Dictionary<string, string>
            {
                { Spreadsheet_CSVFormat, Spreadsheet_CSVFormatContentType },
                { Spreadsheet_OpenFormat, Spreadsheet_OpenFormatContentType },
                { Spreadsheet_ExcelFormat, Spreadsheet_ExcelFormatContentType }
            };
        }
    }
}