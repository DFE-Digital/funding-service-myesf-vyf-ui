using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

namespace PDS.ViewYourFunding.Web.Models.Shared
{
    /// <summary>
    /// Represents a script block on a page.
    /// </summary>
    public class ScriptBlock : IDisposable
    {
        private readonly TextWriter _originalWriter;
        private readonly StringWriter _scriptsWriter;
        private readonly ViewContext _viewContext;

        private const string ScriptsKey = "DelayedScripts";

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptBlock"/> class.
        /// </summary>
        /// <param name="viewContext">The view context to use.</param>
        public ScriptBlock(ViewContext viewContext)
        {
            _viewContext = viewContext;
            _originalWriter = _viewContext.Writer;
            _viewContext.Writer = _scriptsWriter = new StringWriter();
        }

        /// <summary>
        /// Get a list of scripts.
        /// </summary>
        /// <param name="httpContext">The context to use.</param>
        /// <returns>A list of scripts.</returns>
        public static List<string> GetPageScriptsList(HttpContext httpContext)
        {
            if (!(httpContext.Items[ScriptsKey] is List<string> pageScripts))
            {
                pageScripts = new List<string>();
                httpContext.Items[ScriptsKey] = pageScripts;
            }

            return pageScripts;
        }

        /// <summary>
        /// Revert everything back into its original state.
        /// </summary>
        public void Dispose()
        {
            _viewContext.Writer = _originalWriter;

            var pageScripts = GetPageScriptsList(_viewContext.HttpContext);
            pageScripts.Add(_scriptsWriter.ToString());
        }
    }
}