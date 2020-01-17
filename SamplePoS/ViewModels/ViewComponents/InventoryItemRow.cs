using SamplePoS.Core.Models;

namespace SamplePoS.ViewModels.ViewComponents
{
    public class InventoryItemRow: GridRow<InventoryItemRow>
    {
        public Product Product { get; set; }
    }
}
