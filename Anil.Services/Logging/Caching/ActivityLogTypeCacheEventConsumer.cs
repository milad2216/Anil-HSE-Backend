using Anil.Core.Domain.Logging;
using Anil.Services.Caching;
using System.Threading.Tasks;

namespace Anil.Services.Logging.Caching
{
    /// <summary>
    /// Represents a activity log type cache event consumer
    /// </summary>
    public partial class ActivityLogTypeCacheEventConsumer : CacheEventConsumer<ActivityLogType>
    {
    }
}
