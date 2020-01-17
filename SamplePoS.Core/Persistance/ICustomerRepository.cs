using SamplePoS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SamplePoS.Core.Persistance
{
    public interface ICustomerRepository: IRepository<Customer>
    {
       IList<Customer> GetAllCustomers();
       IList<string> GetCustomerNames();
       Customer GetCustomer(Expression<Func<Customer, bool>> expression);
    }
}
