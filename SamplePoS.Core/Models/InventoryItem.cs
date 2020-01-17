using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamplePoS.Core.Models
{
    public class InventoryItem
    {
        [NotMapped]
        private bool disposed;
        [Key]
        public int InventoryItemId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if(Product != null)
                    {
                        Product.Dispose();
                    }
                }

                disposed = true;
            }
        }

        ~InventoryItem()
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
