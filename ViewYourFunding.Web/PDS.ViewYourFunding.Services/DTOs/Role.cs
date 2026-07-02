using System;

namespace PDS.ViewYourFunding.Services.DTOs
{
    /// <summary>
    /// Structure to hold role info returned by DfE Sign-in.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code. These are mapped to GroupName property of each UserRole enum value.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }
    }
}