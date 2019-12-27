using System;
using System.Linq;
using System.Linq.Expressions;

namespace ComicsLibrary.Common.Interfaces
{
    public interface IRepository<TEntity> : IRepository, IQueryable<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Including<TProperty>(Expression<Func<TEntity, TProperty>> property);

        void Insert(TEntity entity);

        void Delete(object id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);

        TEntity GetById(object id);
    }

    public interface IRepository
    {

    }
}