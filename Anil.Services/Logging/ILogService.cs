using System.Collections.Generic;
using System.Threading.Tasks;
using Anil.Core;
using Anil.Core.Domain.Logging;

namespace Anil.Services.Logging
{
    /// <summary>
    /// Provides information about URL records
    /// </summary>
    public partial interface ILogService
    {
        /// <summary>
        /// Deletes an URL records
        /// </summary>
        /// <param name="logs">URL records</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteLogsAsync(IList<Log> logs);

        /// <summary>
        /// Gets an URL records
        /// </summary>
        /// <param name="logIds">URL record identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the uRL record
        /// </returns>
        Task<IList<Log>> GetLogsByIdsAsync(int[] logIds);

        /// <summary>
        /// Inserts an URL record
        /// </summary>
        /// <param name="log">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertLogAsync(Log log);

        /// <summary>
        /// Update an URL record
        /// </summary>
        /// <param name="log">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateLogAsync(Log log);

        /// <summary>
        /// Gets all URL records
        /// </summary>
        /// <param name="slug">Slug</param>
        /// <param name="languageId">Language ID; "null" to load records with any language; "0" to load records with standard language only; otherwise to load records with specify language ID only</param>
        /// <param name="isActive">A value indicating whether to get active records; "null" to load all records; "false" to load only inactive records; "true" to load only active records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the uRL records
        /// </returns>
        Task<IPagedList<Log>> GetAllLogsAsync(string slug = "", bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}