using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Web.Models.Shared;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.Encodings.Web;
using HtmlHelper = Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper;

namespace PDS.ViewYourFunding.Web.Helpers
{
    /// <summary>
    /// Extension methods for <see cref="HtmlHelper"/>. (Copied from Sfs.Web).
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Renders an anchor tag.
        /// </summary>
        /// <param name="helper">The Html helper.</param>
        /// <param name="type">The type of anchor tag.</param>
        /// <param name="href">The href attribute.</param>
        /// <param name="text">The text.</param>
        /// <param name="title">The title attribute.</param>
        /// <param name="id">The id attribute.</param>
        /// <param name="classes">The CSS classes.</param>
        /// <returns>An <see cref="IHtmlContent "/> containing the HTML of the anchor tag.</returns>
        public static IHtmlContent Link(this IHtmlHelper helper, AnchorTagType type, string href, string text, string title = null, string id = null, string classes = null)
        {
            return helper.Link(type, href, new HtmlString(text), title, id, classes);
        }

        /// <summary>
        /// Renders an anchor tag.
        /// </summary>
        /// <param name="helper">The Html helper.</param>
        /// <param name="type">The type of anchor tag.</param>
        /// <param name="href">The href attribute.</param>
        /// <param name="innerHtml">The HTML to inject inside the anchor tag.</param>
        /// <param name="title">The title attribute.</param>
        /// <param name="id">The id attribute.</param>
        /// <param name="classes">The CSS classes.</param>
        /// <returns>An <see cref="IHtmlContent "/> containing the HTML of the anchor tag.</returns>
        public static IHtmlContent Link(this IHtmlHelper helper, AnchorTagType type, string href, IHtmlContent innerHtml, string title = null, string id = null, string classes = null)
        {
            var anchorTagBuilder = new TagBuilder("a");

            anchorTagBuilder.MergeAttribute("href", href);

            if (id != null)
            {
                anchorTagBuilder.MergeAttribute("id", id);
            }

            if (title != null)
            {
                anchorTagBuilder.MergeAttribute("title", title);
            }

            switch (type)
            {
                case AnchorTagType.External:
                    anchorTagBuilder.MergeAttribute("rel", "external noopener noreferrer");
                    anchorTagBuilder.MergeAttribute("title", "This will open in a new tab or window");
                    anchorTagBuilder.MergeAttribute("target", "_blank");
                    break;
                case AnchorTagType.Download:
                    anchorTagBuilder.MergeAttribute("rel", "external");
                    anchorTagBuilder.MergeAttribute("title", "This will prompt a download");
                    break;
                case AnchorTagType.License:
                    anchorTagBuilder.MergeAttribute("rel", "license");
                    break;
            }

            anchorTagBuilder.InnerHtml.AppendHtml(innerHtml.ToString());

            if (!string.IsNullOrEmpty(classes))
            {
                foreach (var className in classes.Split(' '))
                {
                    anchorTagBuilder.AddCssClass(className);
                }
            }

            string rawHtml;
            using (var writer = new StringWriter())
            {
                anchorTagBuilder.WriteTo(writer, HtmlEncoder.Default);

                rawHtml = writer.ToString();
            }

            return new HtmlString(rawHtml);
        }

        /// <summary>
        /// Renders an anchor tag targeting a mailto url for requesting a document in an accessible format.
        /// </summary>
        /// <param name="helper">The Html helper.</param>
        /// <param name="emailAddress">The target email address.</param>
        /// <param name="originalFormat">The original format of the document being requested.</param>
        /// <param name="documentTitle">The title of the document being requested.</param>
        /// <param name="documentPublished">The publish date of the document being requested.</param>
        /// <param name="linkText">The anchor tag text to display .e.g. "Request an accessible format".</param>
        /// <param name="innerHtml">The HTML to inject inside the anchor tag.</param>
        /// <param name="tagTitle">The value to set for the anchor tag title attribute.</param>
        /// <param name="id">The id attribute.</param>
        /// <param name="classes">The CSS classes.</param>
        /// <returns>An <see cref="IHtmlContent "/> containing the HTML of the anchor tag.</returns>
        public static IHtmlContent AccessibleDocumentFormatRequestMailtoLink(
            this IHtmlHelper helper,
            string emailAddress,
            string originalFormat,
            string documentTitle,
            DateTime? documentPublished,
            string linkText,
            IHtmlContent innerHtml = null,
            string tagTitle = null,
            string id = null,
            string classes = null)
        {
            var href = BuildAccessibleDocumentFormatRequestMailtoLinkUrl(emailAddress, originalFormat, documentTitle, documentPublished);

            if (innerHtml == null)
            {
                innerHtml = new HtmlString(linkText);
            }

            return helper.Link(AnchorTagType.Mailto, href, innerHtml, tagTitle, id, classes);
        }

        /// <summary>
        /// Renders a data attribute with the given key and value.
        /// </summary>
        /// <param name="helper">The html helper.</param>
        /// <param name="key">The key to use to build the attribute name.</param>
        /// <param name="value">The string to use as the attribute value.</param>
        /// <returns>An <see cref="IHtmlContent "/> containing the HTML data attribute.</returns>
        public static IHtmlContent DataAttribute(this IHtmlHelper helper, string key, string value)
        {
            return new HtmlString($"data-{key.ToLowerInvariant()}=\"{value}\"");
        }

        /// <summary>
        /// Create a container to define scripts for the page.
        /// </summary>
        /// <param name="helper">The HTML helper to use.</param>
        /// <returns>A disposable object.</returns>
        public static IDisposable BeginScripts(this IHtmlHelper helper)
        {
            return new ScriptBlock(helper.ViewContext);
        }

        /// <summary>
        /// Render page scripts.
        /// </summary>
        /// <param name="helper">The HTML helper to use.</param>
        /// <returns>The HTML of the script tags.</returns>
        public static HtmlString PageScripts(this IHtmlHelper helper)
        {
            return new HtmlString(string.Join(Environment.NewLine, ScriptBlock.GetPageScriptsList(helper.ViewContext.HttpContext).Distinct()));
        }

        /// <summary>
        /// Builds the accessible document format request mailto link url.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="originalFormat">The original format of the document.</param>
        /// <param name="documentTitle">The document title.</param>
        /// <param name="documentPublished">The document published date.</param>
        /// <returns>The mailto link url.</returns>
        private static string BuildAccessibleDocumentFormatRequestMailtoLinkUrl(string emailAddress, string originalFormat, string documentTitle, DateTime? documentPublished)
        {
            var title = RemoveAbbrTags(documentTitle);

            var messageBody = $"Details of document required:\n\n  Title: {title}\n";

            if (documentPublished.HasValue)
            {
                messageBody += $"  Published: {documentPublished.Value.ToDateDisplay()}\n";
            }

            messageBody += $"  Original format: {originalFormat}\n\nPlease tell us:\n\n  1. What makes this format unsuitable for you?\n  2. What format you would prefer?\n      ";

            var message = new MailMessage("a@a.a", emailAddress) // The 'from' address is not used, but must be a valid email address to avoid a runtime error.
            {
                Subject = $"Request for '{title}' in an alternative format",
                Body = messageBody
            };

            return message.AsMailtoUrl();
        }

        /// <summary>
        /// Removes the html abbreviation tag used in the page title for DSG.
        /// </summary>
        /// <param name="value">The string containing the abbreviation tag.</param>
        /// <returns>value with the tag abbr tags removed.</returns>
        private static string RemoveAbbrTags(string value)
        {
            value = value.Replace("Download ", string.Empty);
            value = value.Replace("<abbr title=\"", string.Empty);
            value = value.Replace("\">DSG</abbr>", string.Empty);

            return value;
        }
    }
}
