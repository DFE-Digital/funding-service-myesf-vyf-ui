namespace PDS.VYF.Services.Extensions.Core
{
    using Ardalis.GuardClauses;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    public static class GuardClauseExtensions
    {
        /// <summary>
        /// Validates the Ukrainian Provider Registration Number (UKPRN).
        /// </summary>
        /// <param name="guardClause">The guard clause.</param>
        /// <param name="ukprn">The UKPRN to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>The validated UKPRN.</returns>
        public static string ValidUkrpn(this IGuardClause guardClause, [NotNull][ValidatedNotNull] string? ukprn, [CallerArgumentExpression("ukprn")] string? parameterName = null)
        {
            Guard.Against.NullOrWhiteSpace(ukprn, parameterName);
            Guard.Against.NullOrInvalidInput(
                ukprn,
                parameterName ?? string.Empty,
                a =>
                    {
                        if (int.TryParse(ukprn, out int ukprnNumber))
                        {
                            Guard.Against.NullOrOutOfRange(ukprnNumber, parameterName ?? string.Empty, 10000000, 99999999);
                            return true;
                        }

                        return false;
                    },
                $"{parameterName} should be 8 digit integer.");

            return ukprn;
        }

        /// <summary>
        /// Validates the email address.
        /// </summary>
        /// <param name="guardClause">The guard clause.</param>
        /// <param name="emailId">The email address to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>The validated email address.</returns>
        public static string ValidEmail(this IGuardClause guardClause, [NotNull][ValidatedNotNull] string? emailId, [CallerArgumentExpression("emailId")] string? parameterName = null)
        {
            Guard.Against.NullOrWhiteSpace(emailId, parameterName);
            Guard.Against.NullOrInvalidInput(
                emailId,
                parameterName ?? string.Empty,
                a =>
                {
                    var email = new EmailAddressAttribute();

                    return email.IsValid(emailId);
                },
                $"{parameterName} should be valid Email.");

            return emailId;
        }
    }
}
