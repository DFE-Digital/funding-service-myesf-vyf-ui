using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// The Audit service interface.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Gets Data Import Audits from a query.
        /// </summary>
        /// <returns>Data Import Audit Models.</returns>
        Task<IEnumerable<DataImportAuditModel>> GetDataImportAudit();

        /// <summary>
        /// Gets Data Import Audits from a query.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code.</param>
        /// <returns>Data Import Audit Models.</returns>
        Task<IEnumerable<DataImportAuditModel>> GetDataImportAudit(string fundingStreamCode);
    }
}
