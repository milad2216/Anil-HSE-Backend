using Anil.Core;
using Anil.Core.Infrastructure;
using Anil.Data.Configuration;
using Anil.Data.DataProviders;

namespace Anil.Data
{
    /// <summary>
    /// Represents the data provider manager
    /// </summary>
    public partial class DataProviderManager : IDataProviderManager
    {
        #region Methods

        /// <summary>
        /// Gets data provider by specific type
        /// </summary>
        /// <param name="dataProviderType">Data provider type</param>
        /// <returns></returns>
        public static IAnilDataProvider GetDataProvider(DataProviderType dataProviderType)
        {
            return dataProviderType switch
            {
                DataProviderType.SqlServer => new MsSqlAnilDataProvider(),
                DataProviderType.MySql => new MySqlAnilDataProvider(),
                DataProviderType.PostgreSQL => new PostgreSqlDataProvider(),
                _ => throw new AnilException($"Not supported data provider name: '{dataProviderType}'"),
            };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets data provider
        /// </summary>
        public IAnilDataProvider DataProvider
        {
            get
            {
                var dataProviderType = Singleton<DataConfig>.Instance.DataProvider;

                return GetDataProvider(dataProviderType);
            }
        }

        #endregion
    }
}
