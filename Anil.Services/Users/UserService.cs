using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anil.Core;
using Anil.Core.Caching;
using Anil.Core.Domain.Customers;
using Anil.Core.Domain.Seo;
using Anil.Data;
using Anil.Services.Base;

namespace Anil.Services.Seo
{
    /// <summary>
    /// Provides information about Users
    /// </summary>
    public partial class UserService : BaseService<User>, IUserService
    {
        #region Fields

        private readonly IRepository<User> _userRepository;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        public UserService(IRepository<User> userRepository,
            IStaticCacheManager staticCacheManager,
            IWorkContext workContext): base(userRepository, workContext)
        {
            _userRepository = userRepository;
            _staticCacheManager = staticCacheManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts an User
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertUserAsync(User user)
        {
            await _userRepository.InsertAsync(user);
        }

        /// <summary>
        /// Updates the User
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
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
        public virtual async Task<IPagedList<User>> GetAllUsersAsync(
            bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var urlRecords = (await _userRepository.GetAllAsync(query =>
            {
                query = query.OrderByDescending(ur => ur.CreatedOnUtc);

                return query;
            }, cache => default)).AsQueryable();


            if (isActive.HasValue)
                urlRecords = urlRecords.Where(ur => ur.Active == isActive);

            var result = urlRecords.ToList();

            return new PagedList<User>(result, pageIndex, pageSize);
        }

        public virtual async Task<User?> GetUserByUsernameAndPassword(string username, string password)
        {
            return await _userRepository.Table.FirstOrDefaultAsync(p => p.Username == username && p.Password == password);
        }

        #endregion
    }
}
