using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.EF.Context;
using MD.Bidding.EF.Repository;

namespace MD.Bidding.EF.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : IDbContext, new()
    {
        private IDbContext _context;
        private Dictionary<Type, object> _repositories;
        private bool _disposed;

        public UnitOfWork()
        {
            _context = new TContext();

            _context.Configuration.AutoDetectChangesEnabled = false;

            _repositories = new Dictionary<Type, object>();

            _disposed = false;
        }

        public IRepository<TEntity> Get<TEntity>() where TEntity : class
        {
            if (_repositories.Keys.Contains(typeof(TEntity)))
            {
                return _repositories[typeof(TEntity)] as IRepository<TEntity>;
            }

            var repository = new Repository<TEntity>(_context);

            _repositories.Add(typeof(TEntity), repository);

            return repository;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _context = null;

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}

