using System.Collections.Generic;
using System.Threading.Tasks;
using Anil.Core;
using Anil.Core.Domain.Menus;

namespace Anil.Services.Menus
{
    /// <summary>
    /// Provides information about URL records
    /// </summary>
    public partial interface IMenuService
    {
        /// <summary>
        /// Deletes an URL records
        /// </summary>
        /// <param name="menus">URL records</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteMenusAsync(IList<Menu> menus);

        /// <summary>
        /// Gets an URL records
        /// </summary>
        /// <param name="menuIds">URL record identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the uRL record
        /// </returns>
        Task<IList<Menu>> GetMenusByIdsAsync(int[] menuIds);

        /// <summary>
        /// Inserts an URL record
        /// </summary>
        /// <param name="menu">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertMenuAsync(Menu menu);

        /// <summary>
        /// Update an URL record
        /// </summary>
        /// <param name="menu">URL record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateMenuAsync(Menu menu);

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
        Task<IPagedList<Menu>> GetAllMenusAsync(string slug = "", bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}