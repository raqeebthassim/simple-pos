using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SamplePoS.Core.Models;

namespace SamplePoS.Core.Persistance
{
    public class VendorRepository : Repository<Vendor>, IVendorRepository
    {
        public IList<Vendor> GetAllVendors()
        {
            using (var db = new ApplicationDbContext())
            {
               return this.GetAllEntities(db).ToList();
            }
        }

        public Vendor GetVendor(Expression<Func<Vendor, bool>> expression)
        {
            using (var db = new ApplicationDbContext())
            {
                return this.GetAllEntities(db, expression).FirstOrDefault();
            }
        }

        public IList<string> GetVendorNames()
        {
            using (var db = new ApplicationDbContext())
            {
                return this.GetAllEntities(db).Select(c => c.Name).ToList();
            }
        }
    }
}
