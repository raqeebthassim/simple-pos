using SamplePoS.Core.Models;

namespace SamplePoS.ViewModels.ViewComponents
{
    public class CustomerItemRow : GridRow<CustomerItemRow>
    {
        public Customer Customer { get; set; }
    }
}
