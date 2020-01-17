using SamplePoS.Core.Models;

namespace SamplePoS.ViewModels.ViewComponents
{
    public class VendorItemRow: GridRow<VendorItemRow>
    {
        public Vendor Vendor { get; set; }
    }
}
