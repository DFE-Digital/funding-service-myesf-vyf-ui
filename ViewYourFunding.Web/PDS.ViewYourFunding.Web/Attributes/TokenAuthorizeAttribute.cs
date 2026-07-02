using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Attributes
{
    /// <summary>
    /// Header token based authorisation.
    /// </summary>
    public class TokenAuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the expected auth key.
        /// </summary>
        public static string ExpectedAuthKey { get; set; }

        private const string SECURE_TOKEN_HEADER_NAME = "x-secret-key";

        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!Authorize(context))
            {
                throw new Exception("Authorisation failed");
            }
        }

        private bool Authorize(ActionExecutingContext context)
        {
            if (string.IsNullOrEmpty(ExpectedAuthKey))
            {
                return false;
            }

            try
            {
                var request = context.HttpContext.Request;
                var token = request.Headers.ContainsKey(SECURE_TOKEN_HEADER_NAME) ? request.Headers[SECURE_TOKEN_HEADER_NAME].FirstOrDefault() : null;

                return token == ExpectedAuthKey;
            }
            catch
            {
                return false;
            }
        }
    }
}