using SamplePoS.Core.Models;
using System.Threading.Tasks;

namespace SamplePoS.Core.Persistance
{
    public interface IOrderRepository: IRepository<Order>
    {
        Task AddOrder(Order order);
    }
}
