﻿namespace Anil.Data.Migrations
{
    /// <summary>
    /// Represents default values related to migration process
    /// </summary>
    public static partial class AnilMigrationDefaults
    {
        /// <summary>
        /// Gets the supported datetime formats
        /// </summary>
        public static string[] DateFormats { get; } = { "yyyy-MM-dd HH:mm:ss", "yyyy.MM.dd HH:mm:ss", "yyyy/MM/dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss:fffffff", "yyyy.MM.dd HH:mm:ss:fffffff", "yyyy/MM/dd HH:mm:ss:fffffff" };

        /// <summary>
        /// Gets the format string to create the description of update migration
        /// <remarks>
        /// 0 - anilCommerce full version
        /// 1 - update migration type
        /// </remarks>
        /// </summary>
        public static string UpdateMigrationDescription { get; } = "anilCommerce version {0}. Update {1}";

        /// <summary>
        /// Gets the format string to create the description prefix of update migrations
        /// <remarks>
        /// 0 - anilCommerce full version
        /// </remarks>
        /// </summary>
        public static string UpdateMigrationDescriptionPrefix { get; } = "anilCommerce version {0}. Update";
    }
}
