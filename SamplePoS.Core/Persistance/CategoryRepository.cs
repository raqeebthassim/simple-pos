using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SamplePoS.Core.Models;

namespace SamplePoS.Core.Persistance
{
    public class CategoryRepository: Repository<Category>, ICategoryRepository
    {
        public IList<string> GetCategoryNames()
        {
            using (var db = new ApplicationDbContext())
            {
                return this.GetAllEntities(db).Select(c => c.Name).ToList();
            }
        }

        public IList<Category> GetCategories()
        {
            using (var db = new ApplicationDbContext())
            {
                return this.GetAllEntities(db).ToList();
            }
        }

        public IList<Category> QueryCategories(Expression<Func<Category, bool>> predicate)
        {
            using (var db = new ApplicationDbContext())
            {
                return this.GetAllEntities(db, predicate).ToList();
            }
        }
    }
}
