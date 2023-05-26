using FluentMigrator;
using Anil.Core.Domain.Blogs;
using Anil.Core.Domain.Logging;
using Anil.Core.Domain.Seo;
using Anil.Data.Extensions;
using Anil.Core.Domain.Duties;

namespace Anil.Data.Migrations.Installation
{
    [AnilMigration("2020/01/31 11:24:16:2551771", "Anil.Data base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        /// <summary>
        /// Collect the UP migration expressions
        /// <remarks>
        /// We use an explicit table creation order instead of an automatic one
        /// due to problems creating relationships between tables
        /// </remarks>
        /// </summary>
        public override void Up()
        {
            Create.TableFor<BlogPost>();
            Create.TableFor<BlogPostView>();
            Create.TableFor<Duty>();
            Create.TableFor<BlogComment>();
            Create.TableFor<ActivityLogType>();
            Create.TableFor<ActivityLog>();
            Create.TableFor<Log>();
        }
    }
}
