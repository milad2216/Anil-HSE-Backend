using Anil.Core;
using Anil.Core.Caching;
using Anil.Core.Domain.Menus;
using Anil.Data;

namespace Anil.Services.Menus
{
    /// <summary>
    /// Provides information about URL records
    /// </summary>
    public partial class MenuService : IMenuService
    {
        #region Fields

        private readonly IRepository<Menu> _menuRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public MenuService(IRepository<Menu> menuRepository,
            IStaticCacheManager staticCacheManager,
            IWorkContext workContext)
        {
            _menuRepository = menuRepository;
            _staticCacheManager = staticCacheManager;
            _workContext = workContext;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Deletes an URL records
        /// </summary>
        /// <param name="menus">URL records</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteMenusAsync(IList<Menu> menus)
        {
            await _menuRepository.DeleteAsync(menus);
        }

        /// <summary>
        /// Gets an URL records
        /// </summary>
        /// <param name="menuIds">URL record identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the uRL record
        /// </returns>
        public virtual async Task<IList<Menu>> GetMenusByIdsAsync(int[] menuIds)
        {
            return await _menuRepository.GetByIdsAsync(menuIds, cache => default);
        }

        /// <summary>
        /// Inserts an URL record
        /// </summary>
        /// <param name="menu">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertMenuAsync(Menu menu)
        {
            await _menuRepository.InsertAsync(menu);
        }

        /// <summary>
        /// Updates the URL record
        /// </summary>
        /// <param name="menu">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateMenuAsync(Menu menu)
        {
            await _menuRepository.UpdateAsync(menu);
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
        public virtual async Task<IPagedList<Menu>> GetAllMenusAsync(
            string slug = "", bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var menus = (await _menuRepository.GetAllAsync(query =>
            {
                query = query.OrderBy(ur => ur.ParentMenuId);

                return query;
            }, cache => default)).AsQueryable();

            var result = menus.ToList();

            return new PagedList<Menu>(result, pageIndex, pageSize);
        }

        #endregion
    }
}
