using System.Collections.Generic;
using System.Threading.Tasks;
using Anil.Core;
using Anil.Core.Domain.Customers;
using Anil.Core.Domain.Seo;
using Anil.Services.Base;

namespace Anil.Services.Seo
{
    /// <summary>
    /// Provides information about URL records
    /// </summary>
    public partial interface IUserService: IBaseService<User>
    {

        /// <summary>
        /// Inserts an URL record
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertUserAsync(User user);

        /// <summary>
        /// Update an URL record
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateUserAsync(User user);

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
        Task<IPagedList<User>> GetAllUsersAsync(bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Get User via Username and Password
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Hashed Password</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the User
        /// </returns>
        Task<User?> GetUserByUsernameAndPassword(string username, string password);
    }
}