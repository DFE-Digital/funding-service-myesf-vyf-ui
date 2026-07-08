using PDS.ViewYourFunding.Services.Attributes;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// Methods for working with components.
    /// </summary>
    public interface IComponentService
    {
        /// <summary>
        /// Get a component from the sources.
        /// </summary>
        /// <param name="modelGroup">The ui model group.</param>
        /// <param name="componentConfiguration">The component configuration.</param>
        /// <param name="secondComponentConfiguration">The second component configuration.</param>
        /// <param name="componentDefaults">Defaults for the component.</param>
        /// <param name="publicationDate">The publication date.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="variables">Variables to use.</param>
        /// <returns>Returns a component.</returns>
        public Component GetComponent(
            UiModelGroup modelGroup,
            ComponentConfiguration componentConfiguration,
            ComponentConfiguration secondComponentConfiguration,
            Dictionary<ComponentType, Defaults> componentDefaults,
            DateTime publicationDate,
            string searchTerm,
            Dictionary<string, object> variables);

        /// <summary>
        /// Evaluate a variable.
        /// </summary>
        /// <param name="variable">Variable to evaluate.</param>
        /// <param name="componentConfiguration">The component configuration.</param>
        /// <param name="publicationDate">The publication date.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="variables">Variables to use.</param>
        /// <returns>Returns true, false or an evaluated variable.</returns>
        public object EvaluateVariable(
            UIModelVariable variable,
            ComponentConfiguration componentConfiguration,
            DateTime publicationDate,
            string searchTerm,
            Dictionary<string, object> variables);

        /// <summary>
        /// Get a funding value.
        /// </summary>
        /// <param name="modelGroup">The ui model group.</param>
        /// <param name="componentConfiguration">The component configuration.</param>
        /// <param name="publicationDate">The publication date.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="variables">Variables to use.</param>
        /// <returns>Get the funding value.</returns>
        public object GetFundingValue(
            UiModelGroup modelGroup,
            ComponentConfiguration componentConfiguration,
            DateTime publicationDate,
            string searchTerm,
            Dictionary<string, object> variables);
    }
}