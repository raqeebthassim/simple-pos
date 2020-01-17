using SamplePoS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SamplePoS.Core.Persistance
{
    public interface IVendorRepository: IRepository<Vendor>
    {
        IList<string> GetVendorNames();
        IList<Vendor> GetAllVendors();
        Vendor GetVendor(Expression<Func<Vendor, bool>> expression);
    }
}
