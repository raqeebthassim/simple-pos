using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SamplePoS.Core.Persistance
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        public IQueryable<TEntity> GetAllEntities(ApplicationDbContext db)
        {
            return db.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAllEntities(ApplicationDbContext db, Expression<Func<TEntity, bool>> predicate)
        {
                return db.Set<TEntity>().Where(predicate);
        }
       

        public async Task Create(TEntity entity)
        {
            using(var db = new ApplicationDbContext())
            {
                db.Set<TEntity>().Update(entity);
                await db.SaveChangesAsync();
            }
        }

        public async Task Update(TEntity entity)
        {
            using(var db = new ApplicationDbContext())
            {
                var updatedEntity = db.Set<TEntity>().Update(entity);
                await db.SaveChangesAsync();
            }
        }

        public async Task Delete(TEntity entity)
        {
            using(var db = new ApplicationDbContext())
            {
                db.Set<TEntity>().Remove(entity);
                await db.SaveChangesAsync();
            }
        }
    }
}
