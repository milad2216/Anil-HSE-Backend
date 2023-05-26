using System.Collections.Generic;
using System.Threading.Tasks;
using Anil.Core;
using Anil.Core.Domain.Logging;

namespace Anil.Services.Logging
{
    /// <summary>
    /// Provides information about URL records
    /// </summary>
    public partial interface IActivityLogTypeService
    {
        /// <summary>
        /// Deletes an URL records
        /// </summary>
        /// <param name="activityLogTypes">URL records</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteActivityLogTypesAsync(IList<ActivityLogType> activityLogTypes);

        /// <summary>
        /// Gets an URL records
        /// </summary>
        /// <param name="activityLogTypeIds">URL record identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the uRL record
        /// </returns>
        Task<IList<ActivityLogType>> GetActivityLogTypesByIdsAsync(int[] activityLogTypeIds);

        /// <summary>
        /// Inserts an URL record
        /// </summary>
        /// <param name="activityLogType">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertActivityLogTypeAsync(ActivityLogType activityLogType);

        /// <summary>
        /// Update an URL record
        /// </summary>
        /// <param name="activityLogType">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateActivityLogTypeAsync(ActivityLogType activityLogType);

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
        Task<IPagedList<ActivityLogType>> GetAllActivityLogTypesAsync(string slug = "", bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}