using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamplePoS.Core.Models
{
    public class PurchaseOrder: IDisposable
    {
        [NotMapped]
        private bool disposed;

        [Key]
        public int PurchaseOrderId { get; set; }
        public DateTime Time { get; set; }
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public decimal Total { get; set; }
        public bool Paid { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if(Vendor != null)
                    {
                        Vendor.Dispose();
                    }
                    if(OrderItems != null && OrderItems.Count > 0)
                    {
                        foreach(OrderItem o in OrderItems)
                        {
                            o.Dispose();
                        }
                    }
                }

                disposed = true;
            }
        }

        ~PurchaseOrder()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
