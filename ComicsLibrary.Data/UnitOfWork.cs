using ComicsLibrary.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComicsLibrary.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private readonly Dictionary<Type, IRepository> _repositories = new Dictionary<Type, IRepository>();

        public IRepository<T> Repository<T>() where T : class
        {
            if (!_repositories.Keys.Contains(typeof(T)))
            {
                _repositories.Add(typeof(T), new Repository<T>(_context));
            }

            return _repositories[typeof(T)] as IRepository<T>;
        }
        
        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}