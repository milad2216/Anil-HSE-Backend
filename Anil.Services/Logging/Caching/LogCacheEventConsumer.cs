using Anil.Core.Domain.Logging;
using Anil.Services.Caching;

namespace Anil.Services.Logging.Caching
{
    /// <summary>
    /// Represents a log cache event consumer
    /// </summary>
    public partial class LogCacheEventConsumer : CacheEventConsumer<Log>
    {
    }
}
