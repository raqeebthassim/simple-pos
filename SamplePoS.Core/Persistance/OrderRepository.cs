
using System.Threading.Tasks;
using SamplePoS.Core.Models;

namespace SamplePoS.Core.Persistance
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public async Task AddOrder(Order order)
        {
            using (var db = new ApplicationDbContext())
            {
                await this.Create(order);
            }
        }
    }
}
