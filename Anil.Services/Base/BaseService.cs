using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Anil.Core;
using Anil.Core.Caching;
using Anil.Core.Common;
using Anil.Data;
using Microsoft.EntityFrameworkCore;
using static Anil.Core.Infrastructure.Enums.Types;

namespace Anil.Services.Base
{
    public class BaseService<TEntity> : BaseEntity where TEntity : BaseEntity
    {
        private IRepository<TEntity> _repository;
        protected readonly IWorkContext _workContext;
        public BaseService(IRepository<TEntity> repository,
            IWorkContext workContext)
        {
            _repository = repository;
            _workContext = workContext;
        }
        public IQueryable<TEntity> Entities
        {
            get
            {
                return _repository.Table;
            }
        }

        public virtual CFResult Edit(TEntity model)
        {
            //_repository.Entity.Attach(model);
            _repository.Update(model);


            return new CFResult
            {
                Status = OperationResultType.Success,
                Message = "ویرایش با موفقیت انجام شد.",
                Extra = model
            };
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _repository.Table;
        }

        public virtual IList<TEntity> GetAllList(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            return _repository.GetAll(func, getCacheKey: getCacheKey);
        }

        public virtual void Dispose()
        {
            _repository.Dispose();
        }

        public virtual List<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.Table.Where(predicate).ToList();
        }


        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.Table.Where(predicate);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.Table.Count(predicate);
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.Table.Any(predicate);
        }

        public int DeleteExp(Expression<Func<TEntity, bool>> filterExpression)
        {
            return _repository.Delete(filterExpression);
        }

        public static string ArabicCharsToPersian(object val)
        {
            var value = (string)val;
            value = value.Replace('ي', 'ی');
            value = value.Replace('ك', 'ک');
            value = value.Replace('٤', '۴');
            value = value.Replace('٥', '۵');
            value = value.Replace('٦', '٦');
            return value;
        }
        public virtual CFResult Delete(TEntity entity)
        {
            _repository.Delete(entity);
            return new CFResult
            {
                Status = OperationResultType.Success,
                Message = "رکورد مورد نظر با موفقیت حذف شد."
            };
        }

        public virtual TEntity FindById(int id)
        {
            return _repository.GetById(id, cache => default);
        }

        public virtual CFResult Create(TEntity model)
        {
            _repository.Insert(model);
            return new CFResult
            {
                Status = OperationResultType.Success,
                Message = "رکورد با موفقیت ایجاد شد.",
                ID = model.Id.ToString(),
                Extra = model
            };
        }

    }
}
