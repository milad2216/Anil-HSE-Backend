using Anil.Core;
using Anil.Core.Caching;
using Anil.Core.Domain.Logging;
using Anil.Data;

namespace Anil.Services.Logging
{
    /// <summary>
    /// Provides information about URL records
    /// </summary>
    public partial class ActivityLogService : IActivityLogService
    {
        #region Fields

        private readonly IRepository<ActivityLog> _activityLogRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public ActivityLogService(IRepository<ActivityLog> activityLogRepository,
            IStaticCacheManager staticCacheManager,
            IWorkContext workContext)
        {
            _activityLogRepository = activityLogRepository;
            _staticCacheManager = staticCacheManager;
            _workContext = workContext;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Deletes an URL records
        /// </summary>
        /// <param name="activityLogs">URL records</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteActivityLogsAsync(IList<ActivityLog> activityLogs)
        {
            await _activityLogRepository.DeleteAsync(activityLogs);
        }

        /// <summary>
        /// Gets an URL records
        /// </summary>
        /// <param name="activityLogIds">URL record identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the uRL record
        /// </returns>
        public virtual async Task<IList<ActivityLog>> GetActivityLogsByIdsAsync(int[] activityLogIds)
        {
            return await _activityLogRepository.GetByIdsAsync(activityLogIds, cache => default);
        }

        /// <summary>
        /// Inserts an URL record
        /// </summary>
        /// <param name="activityLog">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertActivityLogAsync(ActivityLog activityLog)
        {
            await _activityLogRepository.InsertAsync(activityLog);
        }

        /// <summary>
        /// Updates the URL record
        /// </summary>
        /// <param name="activityLog">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateActivityLogAsync(ActivityLog activityLog)
        {
            await _activityLogRepository.UpdateAsync(activityLog);
        }

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
        public virtual async Task<IPagedList<ActivityLog>> GetAllActivityLogsAsync(
            string slug = "", bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var activityLogs = (await _activityLogRepository.GetAllAsync(query =>
            {
                query = query.OrderBy(ur => ur.Id);

                return query;
            }, cache => default)).AsQueryable();

            var result = activityLogs.ToList();

            return new PagedList<ActivityLog>(result, pageIndex, pageSize);
        }

        #endregion
    }
}
