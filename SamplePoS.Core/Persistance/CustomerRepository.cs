using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SamplePoS.Core.Models;

namespace SamplePoS.Core.Persistance
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public IList<Customer> GetAllCustomers()
        {
            using (var db = new ApplicationDbContext())
            {
                return this.GetAllEntities(db).ToList();
            }
        }

        public IList<string> GetCustomerNames()
        {
            using(var db = new ApplicationDbContext())
            {
                return this.GetAllEntities(db).Select(c => c.Name).ToList();
            }
        }

        public Customer GetCustomer(Expression<Func<Customer, bool>> expression)
        {
            using (var db = new ApplicationDbContext())
            {
                return this.GetAllEntities(db, expression).FirstOrDefault();
            }
        }
    }
}
