using SamplePoS.Core.Models;
using System.Collections.Generic;

namespace SamplePoS.Core.Persistance
{
    public interface IProductRepository: IRepository<Product>
    {
        IList<Product> GetProductList();
        IList<string> GetProductNames();
    }
}
