using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using static System.Uri;

namespace PDS.ViewYourFunding.Web.Helpers
{
    /// <summary>
    /// Extension methods for the class: <see cref="MailMessage"/>.
    /// </summary>
    public static class MailMessageExtensions
    {
        /// <summary>
        /// Converts the email message to a 'mailto' url.
        /// </summary>
        /// <param name="message">The email message to convert.</param>
        /// <returns>The 'mailto' url representing the email message.</returns>
        public static string AsMailtoUrl(this MailMessage message) =>
            $"mailto:{Recipients(message.To)}?" + string.Join("&", Parameters(message));

        /// <summary>
        /// Gets the link parameters from the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The link parameters.</returns>
        private static IEnumerable<string> Parameters(MailMessage message)
        {
            var parameters = new List<string>();

            if (message.CC.Any())
            {
                parameters.Add("cc=" + Recipients(message.CC));
            }

            if (message.Bcc.Any())
            {
                parameters.Add("bcc=" + Recipients(message.Bcc));
            }

            if (!string.IsNullOrWhiteSpace(message.Subject))
            {
                parameters.Add("subject=" + EscapeDataString(message.Subject));
            }

            if (!string.IsNullOrWhiteSpace(message.Body))
            {
                parameters.Add("body=" + EscapeDataString(message.Body));
            }

            return parameters;
        }

        /// <summary>
        /// Gets the recipients from the addresses.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <returns>A delimited string containing the recipients.</returns>
        private static string Recipients(MailAddressCollection addresses) =>
            string.Join(
                ",",
                from r in addresses select EscapeDataString(r.Address));
    }
}
