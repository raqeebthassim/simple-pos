using SamplePoS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SamplePoS.Core.Persistance
{
    public interface ICategoryRepository: IRepository<Category>
    {
        IList<string> GetCategoryNames();
        IList<Category> GetCategories();
        IList<Category> QueryCategories(Expression<Func<Category, bool>> predicate);
    }
}
