using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.EF.Repository;
using MD.Bidding.EF.Context;

namespace MD.Bidding.EF.UnitOfWork
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : IDbContext
    {
        IRepository<TEntity> Get<TEntity>() where TEntity : class;
        int SaveChanges();
    }
}
