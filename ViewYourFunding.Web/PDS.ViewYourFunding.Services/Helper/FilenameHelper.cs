using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SearchFilter = PDS.ViewYourFunding.Services.DTOs.SearchFilter;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Helper functions for dealing with filenames.
    /// </summary>
    public static class FilenameHelper
    {
        /// <summary>
        /// Create a filename from component parts.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. AY-1920).</param>
        /// <param name="publicationDate">Publication date and time to use as part of the filename.</param>
        /// <param name="fileFormat">File format e.g. csv, ods. by default use a popular vendor specific version (e.g. XLS 2003).</param>
        /// <param name="fundingDocumentType">The funding document type (e.g. Spreadsheet).</param>
        /// <param name="fundingDocumentScope">The funding document scope (e.g. National).</param>
        /// <param name="filters">Optional filters to use when fetching data.</param>
        /// <returns>A filename in the defined format.</returns>
        public static string BuildFundingDocumentFilename(
            string fundingStreamCode,
            string fundingPeriodCode,
            DateTime publicationDate,
            FileFormat fileFormat,
            FundingViewType fundingDocumentType,
            FundingViewScope fundingDocumentScope,
            SearchFilter[] filters = null)
        {
            string extension;

            switch (fundingDocumentType)
            {
                case FundingViewType.Spreadsheet:
                    extension = fileFormat.ToString().ToLower();
                    break;
                default:
                    throw new Exception("Document type not supported");
            }

            var publicationDateString = publicationDate.ToString("yyyyMMdd_HHmmss");
            var filtersString = string.Empty;

            if (filters != null && filters.Any())
            {
                foreach (var searchFilter in filters)
                {
                    filtersString += $"_{searchFilter.PropertyValue}";
                }
            }

            var scope = string.Empty;

            if (fundingDocumentScope != FundingViewScope.National)
            {
                scope = $"_{fundingDocumentScope.ToString()}";
            }

            return $"{BuildFundingDocumentFilenamePrefix(fundingStreamCode, fundingPeriodCode)}{publicationDateString}{scope}{filtersString}.{extension}";
        }

        /// <summary>
        /// Get the start of a funding document filename.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. AY-1920).</param>
        /// <returns>The first part of a filename.</returns>
        public static string BuildFundingDocumentFilenamePrefix(string fundingStreamCode, string fundingPeriodCode)
        {
            return $"{fundingStreamCode}_{fundingPeriodCode}_";
        }

        /// <summary>
        /// Create a output spreadsheet filename from an internal one.
        /// </summary>
        /// <param name="internalFilename">The internal filename of the file.</param>
        /// <param name="publishedDate">The published date of the document.</param>
        /// <param name="configuration">The funding stream configuration dictionary.</param>
        /// <returns>An outputable filename.</returns>
        public static string BuildOutputSpreadsheetFilename(
            string internalFilename,
            DateTime publishedDate,
            List<FundingStream> configuration)
        {
            var fileNameComponents = GetComponentsFromFilename(internalFilename);

            if (fileNameComponents == null)
            {
                throw new Exception("Filename format is not valid");
            }

            var fundingStreamName = configuration.FirstOrDefault(fs => fs.FundingStreamCode == fileNameComponents.FundingStreamCode)?.FundingStreamName;

            return BuildOutputSpreadsheetFilename(
                fundingStreamName,
                fileNameComponents.FundingPeriodCode,
                publishedDate,
                null,
                fileNameComponents.Extension);
        }

        /// <summary>
        /// Create a output spreadsheet filename from an internal one.
        /// </summary>
        /// <param name="fundingStreamName">The funding stream name (e.g. Dedicated schools grant).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. AY-1920).</param>
        /// <param name="publishedDate">The published date of the document.</param>
        /// <param name="extension">The extension of the file (e.g. ODS).</param>
        /// <returns>An outputable filename.</returns>
        public static string BuildOutputSpreadsheetFilename(
            string fundingStreamName,
            string fundingPeriodCode,
            DateTime publishedDate,
            string extension)
        {
            return BuildOutputSpreadsheetFilename(
                fundingStreamName,
                fundingPeriodCode,
                publishedDate,
                null,
                extension);
        }

        /// <summary>
        /// Create a output spreadsheet filename from an internal one.
        /// </summary>
        /// <param name="fundingStreamName">The funding stream name (e.g. Dedicated schools grant).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. AY-1920).</param>
        /// <param name="publishedDate">The published date of the document.</param>
        /// <param name="organisationName">The organisation name.</param>
        /// <param name="extension">The extension of the file (e.g. ODS).</param>
        /// <returns>An outputable filename.</returns>
        public static string BuildOutputSpreadsheetFilename(
            string fundingStreamName,
            string fundingPeriodCode,
            DateTime publishedDate,
            string organisationName,
            string extension)
        {
            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(fundingPeriodCode);

            return $"{fundingStreamName?.ToLower().Sanitise('-')}" +
                $"_{yearFrom}-to-{yearTo}" +
                $"_published-{publishedDate.ToFilenameString()}" +
                (string.IsNullOrEmpty(organisationName) ? string.Empty : $"_{organisationName.ToLower().Sanitise('-')}") +
                $".{extension.ToLower()}";
        }

        /// <summary>
        /// Get components from a filename.
        /// </summary>
        /// <param name="filename">The filename to get the components from.</param>
        /// <returns>The component parts of the filename.</returns>
        public static FileNameComponents GetComponentsFromFilename(string filename)
        {
            var parts = filename.Split('_');

            if (parts.Length < 4)
            {
                return null;
            }

            var fundingStreamCode = parts[0];
            var fundingPeriodCode = parts[1];

            var dateStr = parts[2] + parts[3].Split('.')[0];

            if (!int.TryParse(dateStr.Substring(0, 4), out var year)
                || !int.TryParse(dateStr.Substring(4, 2), out var month)
                || !int.TryParse(dateStr.Substring(6, 2), out var day))
            {
                return null;
            }

            var lastPart = parts[parts.Length - 1];
            var extension = lastPart.Contains(".") ? lastPart.Split('.')[1] : null;

            return new FileNameComponents
            {
                FundingStreamCode = fundingStreamCode,
                FundingPeriodCode = fundingPeriodCode,
                PublicationDate = new DateTime(year, month, day),
                Extension = extension
            };
        }
    }
}