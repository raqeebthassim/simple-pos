using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SamplePoS.Core.Persistance
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAllEntities(ApplicationDbContext db);
        IQueryable<TEntity> GetAllEntities(ApplicationDbContext db, Expression<Func<TEntity, bool>> predicate);

        Task Create(TEntity entity);

        Task Delete(TEntity entity);
    }
}
