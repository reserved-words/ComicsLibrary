using System;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace ComicsLibrary.Common.Interfaces
{
    public interface IRepository<TEntity> : IRepository, IQueryable<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Including<TProperty>(Expression<Func<TEntity, TProperty>> property);
        IQueryable<TEntity> Including<TProperty1, TProperty2>(Expression<Func<TEntity, TProperty1>> property1, Expression<Func<TEntity, TProperty2>> property2);

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