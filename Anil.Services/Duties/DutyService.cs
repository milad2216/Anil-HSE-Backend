using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anil.Core;
using Anil.Core.Caching;
using Anil.Core.Domain.Customers;
using Anil.Core.Domain.Duties;
using Anil.Core.Domain.Seo;
using Anil.Data;
using Anil.Services.Base;

namespace Anil.Services.Duties
{
    /// <summary>
    /// Provides information about Users
    /// </summary>
    public partial class DutyService : BaseService<Duty>, IDutyService
    {
        #region Fields

        private readonly IRepository<Duty> _dutyRepository;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        public DutyService(IRepository<Duty> dutyRepository,
            IStaticCacheManager staticCacheManager,
            IWorkContext workContext): base(dutyRepository, workContext)
        {
            _dutyRepository = dutyRepository;
            _staticCacheManager = staticCacheManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts an User
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertUserAsync(Duty user)
        {
            await _dutyRepository.InsertAsync(user);
        }

        /// <summary>
        /// Updates the User
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateUserAsync(Duty user)
        {
            await _dutyRepository.UpdateAsync(user);
        }

        /// <summary>
        /// Gets all Users
        /// </summary>
        /// <param name="isActive">A value indicating whether to get active users; "null" to load all records; "false" to load only inactive records; "true" to load only active records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the users
        /// </returns>
        public virtual async Task<IPagedList<Duty>> GetAllUsersAsync(
            bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var duties = (await _dutyRepository.GetAllAsync(query =>
            {
                query = query.OrderByDescending(ur => ur.CreatedOnUtc);

                return query;
            }, cache => default)).AsQueryable();


            if (isActive.HasValue)
                duties = duties.Where(ur => ur.Active == isActive);

            var result = duties.ToList();

            return new PagedList<Duty>(result, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets and cache six selected duty for main page
        /// </summary>
        /// <returns>
        /// The result contains the duties
        /// </returns>
        public virtual List<Duty> GetMainPageDuties()
        {
            return _staticCacheManager.Get<List<Duty>>(AnilDutyDefaults.LastSixDutyKey, () =>
            {
                return _dutyRepository.GetAll().Where(p => p.ShowInTopSix == true).OrderByDescending(o => o.CreatedOnUtc).Take(6).ToList();
            });
        }

        #endregion
    }
}
