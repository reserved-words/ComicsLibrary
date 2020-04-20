using ComicsLibrary.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;

namespace ComicsLibrary.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        public Repository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Including<TProperty>(Expression<Func<TEntity, TProperty>> property)
        {
            return dbSet.Include(property);
        }

        public virtual IQueryable<TEntity> Including<TProperty1, TProperty2>(
            Expression<Func<TEntity, TProperty1>> property1,
            Expression<Func<TEntity, TProperty2>> property2)
        {
            return dbSet.Include(property1).Include(property2);
        }


        public virtual IQueryable<TEntity> Including<TProperty>(params Expression<Func<TEntity, TProperty>>[] properties)
        {
            var set = dbSet.Include(properties[0]);

            for (var i = 1; i < properties.Count(); i++)
            {
                set = set.Include(properties[i]);
            }

            return set;
        }

        public TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entity)
        {
            var dbEntity = context.Entry(entity);

            if (dbEntity.State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            dbEntity.State = EntityState.Modified;
        }

        public Expression Expression => dbSet.AsQueryable().Expression;

        public Type ElementType => dbSet.AsQueryable().ElementType;

        public IQueryProvider Provider => dbSet.AsQueryable().Provider;

        public IEnumerator<TEntity> GetEnumerator()
        {
            return dbSet.AsQueryable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dbSet.AsQueryable().GetEnumerator();
        }

        public IQueryable<TEntity> GetFromSql(string sql, params SqlParameter[] parameters)
        {
            return context.Set<TEntity>().FromSqlRaw(sql, parameters);
        }
    }
}
