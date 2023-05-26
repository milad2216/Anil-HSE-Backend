using Anil.Core;
using Anil.Core.Caching;
using Anil.Core.Domain.Logging;
using Anil.Data;

namespace Anil.Services.Logging
{
    /// <summary>
    /// Provides information about URL records
    /// </summary>
    public partial class LogService : ILogService
    {
        #region Fields

        private readonly IRepository<Log> _logRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public LogService(IRepository<Log> logRepository,
            IStaticCacheManager staticCacheManager,
            IWorkContext workContext)
        {
            _logRepository = logRepository;
            _staticCacheManager = staticCacheManager;
            _workContext = workContext;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Deletes an URL records
        /// </summary>
        /// <param name="logs">URL records</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteLogsAsync(IList<Log> logs)
        {
            await _logRepository.DeleteAsync(logs);
        }

        /// <summary>
        /// Gets an URL records
        /// </summary>
        /// <param name="logIds">URL record identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the uRL record
        /// </returns>
        public virtual async Task<IList<Log>> GetLogsByIdsAsync(int[] logIds)
        {
            return await _logRepository.GetByIdsAsync(logIds, cache => default);
        }

        /// <summary>
        /// Inserts an URL record
        /// </summary>
        /// <param name="log">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertLogAsync(Log log)
        {
            await _logRepository.InsertAsync(log);
        }

        /// <summary>
        /// Updates the URL record
        /// </summary>
        /// <param name="log">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateLogAsync(Log log)
        {
            await _logRepository.UpdateAsync(log);
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
        public virtual async Task<IPagedList<Log>> GetAllLogsAsync(
            string slug = "", bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var logs = (await _logRepository.GetAllAsync(query =>
            {
                query = query.OrderBy(ur => ur.Id);

                return query;
            }, cache => default)).AsQueryable();

            var result = logs.ToList();

            return new PagedList<Log>(result, pageIndex, pageSize);
        }

        #endregion
    }
}
