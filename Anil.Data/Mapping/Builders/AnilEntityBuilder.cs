﻿using FluentMigrator.Builders.Create.Table;
using Anil.Core;

namespace Anil.Data.Mapping.Builders
{
    /// <summary>
    /// Represents base entity builder
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <remarks>
    /// "Entity type <see cref="TEntity"/>" is needed to determine the right entity builder for a specific entity type
    /// </remarks>
    public abstract class AnilEntityBuilder<TEntity> : IEntityBuilder where TEntity : BaseEntity
    {
        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public abstract void MapEntity(CreateTableExpressionBuilder table);
    }
}