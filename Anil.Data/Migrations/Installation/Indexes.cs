using FluentMigrator;
using FluentMigrator.SqlServer;
using Anil.Core.Domain.Logging;
using Anil.Core.Domain.Seo;
using Anil.Data.Mapping;

namespace Anil.Data.Migrations.Installation
{
    [AnilMigration("2020/03/13 09:36:08:9037677", "Anil.Data base indexes", MigrationProcessType.Installation)]
    public class Indexes : AutoReversingMigration
    {
        #region Methods

        public override void Up()
        {
            Create.Index("IX_UrlRecord_Slug")
                .OnTable(nameof(UrlRecord))
                .OnColumn(nameof(UrlRecord.Slug))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create.Index("IX_UrlRecord_Custom_1").OnTable(nameof(UrlRecord))
                .OnColumn(nameof(UrlRecord.EntityId)).Ascending()
                .OnColumn(nameof(UrlRecord.EntityName)).Ascending()
                .OnColumn(nameof(UrlRecord.IsActive)).Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_ActivityLog_CreatedOnUtc").OnTable(nameof(ActivityLog))
                .OnColumn(nameof(ActivityLog.CreatedOnUtc)).Descending()
                .WithOptions().NonClustered();
        }

        #endregion
    }
}
