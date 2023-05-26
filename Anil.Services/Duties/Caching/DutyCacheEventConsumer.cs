using Anil.Core.Domain.Duties;
using Anil.Core.Domain.Menus;
using Anil.Services.Caching;
using Anil.Services.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anil.Services.Duties.Caching
{
    public partial class DutyCacheEventConsumer : CacheEventConsumer<Duty>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(Duty entity)
        {
            await base.ClearCacheAsync(entity);
            await RemoveAsync(AnilDutyDefaults.LastSixDutyKey);
        }
    }
}
