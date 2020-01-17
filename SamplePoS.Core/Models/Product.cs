using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamplePoS.Core.Models
{
    public class Product: IDisposable
    {
        [NotMapped]
        private bool disposed;

        [Key]
        public int ProductId { get; set; }
        public int? VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Quantity { get; set; }

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
                    
                    if(Category != null)
                    {
                        Category.Dispose();
                    }
                }

                disposed = true;
            }
        }

        ~Product()
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
