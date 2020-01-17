
using Prism.Commands;
using SamplePoS.Core.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SamplePoS.ViewModels.ViewComponents
{
    public class OrderItemListView: GridRow<OrderItemListView>, IDisposable
    {
        public OrderItem OrderItem { get; set; }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    OrderItem.Dispose();
                }

                disposed = true;
            }
        }

        ~OrderItemListView()
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

