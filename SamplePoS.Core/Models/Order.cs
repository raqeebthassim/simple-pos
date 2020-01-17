using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamplePoS.Core.Models
{
    public class Order
    {
        [NotMapped]
        private bool disposed;
        [Key]
        public int OrderId { get; set; }
        public DateTime Time { get; set; }

        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if(Customer != null)
                    {
                        Customer.Dispose();
                    }
                    if(OrderItems != null && OrderItems.Count > 0)
                    {
                        foreach (var o in OrderItems)
                        {
                            o.Dispose();
                        }
                    }
                }

                disposed = true;
            }
        }

        ~Order()
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
