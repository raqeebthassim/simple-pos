using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamplePoS.Core.Models
{
    public class OrderItem
    {
        [NotMapped]
        private bool disposed;

        [Key]
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal LineDiscount { get; set; }
        public decimal Quantity { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public decimal SubTotal { get; set; }

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
                    if(Order != null)
                    {
                        Order.Dispose();
                    }
                }

                disposed = true;
            }
        }

        ~OrderItem()
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
