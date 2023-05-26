using System.Collections.Generic;
using System.Threading.Tasks;
using Anil.Core;
using Anil.Core.Domain.Customers;
using Anil.Core.Domain.Duties;
using Anil.Core.Domain.Seo;
using Anil.Services.Base;

namespace Anil.Services.Duties
{
    /// <summary>
    /// Provides information about URL records
    /// </summary>
    public partial interface IDutyService: IBaseService<Duty>
    {

        /// <summary>
        /// Inserts an URL record
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertUserAsync(Duty user);

        /// <summary>
        /// Update an URL record
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateUserAsync(Duty user);

        /// <summary>
        /// Gets all Users
        /// </summary>
        /// <param name="isActive">A value indicating whether to get active records; "null" to load all records; "false" to load only inactive records; "true" to load only active records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the uRL records
        /// </returns>
        Task<IPagedList<Duty>> GetAllUsersAsync(bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets and cache six selected duty for main page
        /// </summary>
        /// <returns>
        /// The result contains the duties
        /// </returns>
        List<Duty> GetMainPageDuties();
    }
}