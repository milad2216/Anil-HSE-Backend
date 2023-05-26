using Anil.Core;
using Anil.Core.Caching;
using Anil.Core.Domain.Logging;
using Anil.Data;

namespace Anil.Services.Logging
{
    /// <summary>
    /// Provides information about URL records
    /// </summary>
    public partial class ActivityLogTypeService : IActivityLogTypeService
    {
        #region Fields

        private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public ActivityLogTypeService(IRepository<ActivityLogType> activityLogTypeRepository,
            IStaticCacheManager staticCacheManager,
            IWorkContext workContext)
        {
            _activityLogTypeRepository = activityLogTypeRepository;
            _staticCacheManager = staticCacheManager;
            _workContext = workContext;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Deletes an URL records
        /// </summary>
        /// <param name="activityLogTypes">URL records</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteActivityLogTypesAsync(IList<ActivityLogType> activityLogTypes)
        {
            await _activityLogTypeRepository.DeleteAsync(activityLogTypes);
        }

        /// <summary>
        /// Gets an URL records
        /// </summary>
        /// <param name="activityLogTypeIds">URL record identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the uRL record
        /// </returns>
        public virtual async Task<IList<ActivityLogType>> GetActivityLogTypesByIdsAsync(int[] activityLogTypeIds)
        {
            return await _activityLogTypeRepository.GetByIdsAsync(activityLogTypeIds, cache => default);
        }

        /// <summary>
        /// Inserts an URL record
        /// </summary>
        /// <param name="activityLogType">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertActivityLogTypeAsync(ActivityLogType activityLogType)
        {
            await _activityLogTypeRepository.InsertAsync(activityLogType);
        }

        /// <summary>
        /// Updates the URL record
        /// </summary>
        /// <param name="activityLogType">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateActivityLogTypeAsync(ActivityLogType activityLogType)
        {
            await _activityLogTypeRepository.UpdateAsync(activityLogType);
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
        public virtual async Task<IPagedList<ActivityLogType>> GetAllActivityLogTypesAsync(
            string slug = "", bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var activityLogTypes = (await _activityLogTypeRepository.GetAllAsync(query =>
            {
                query = query.OrderBy(ur => ur.Id);

                return query;
            }, cache => default)).AsQueryable();

            var result = activityLogTypes.ToList();

            return new PagedList<ActivityLogType>(result, pageIndex, pageSize);
        }

        #endregion
    }
}
