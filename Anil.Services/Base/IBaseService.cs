using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Anil.Core;
using Anil.Core.Caching;
using Anil.Core.Common;

namespace Anil.Services.Base
{
    public interface IBaseService<TModel> : IDisposable where TModel : BaseEntity
    {
        IQueryable<TModel> Entities { get; }
        CFResult Edit(TModel model);
        IQueryable<TModel> GetAll();
        IList<TModel> GetAllList(Func<IQueryable<TModel>, IQueryable<TModel>> func = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null);
        int Count(Expression<Func<TModel, bool>> predicate);

        bool Any(Expression<Func<TModel, bool>> predicate);

        List<TModel> FindBy(Expression<Func<TModel, bool>> predicate);

        IQueryable<TModel> Where(Expression<Func<TModel, bool>> predicate);

        int DeleteExp(Expression<Func<TModel, bool>> filterExpression);

        CFResult Delete(TModel id);

        TModel FindById(int id);

        CFResult Create(TModel model);
    }
}
