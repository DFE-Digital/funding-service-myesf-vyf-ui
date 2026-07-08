using System;

namespace PDS.ViewYourFunding.Services.Attributes
{
    /// <summary>
    /// An attribute to determine if this funding view scope is for logged in views.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class LoggedInViewAttribute : Attribute
    {
    }
}