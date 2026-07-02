using System;

namespace PDS.ViewYourFunding.Web.Exceptions
{
    /// <summary>
    /// Request Exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class RequestException : Exception
    {
        public RequestException(string errorMessage)
            : base(errorMessage)
        {
        }
    }
}