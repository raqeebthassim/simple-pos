using System.Collections.Generic;
using SamplePoS.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SamplePoS.Core.Persistance
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        public IList<Product> GetProductList()
        {
            using(var db = new ApplicationDbContext())
            {
                return GetAllEntities(db)
                    .Include(a => a.Category)
                    .Include(a => a.Vendor)
                    .ToList();
            }
        }

        public IList<string> GetProductNames()
        {
            using(var db = new ApplicationDbContext())
            {
                return GetAllEntities(db).Select(p => p.Name).ToList();
            }
        }
    }
}
