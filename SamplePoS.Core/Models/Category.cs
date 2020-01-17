using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamplePoS.Core.Models
{
    public class Category: IDisposable
    {
        [NotMapped]
        private bool disposed;

        [Key]
        public int CategoryId { get; set;}
        public string Name { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose-only, i.e. non-finalizable logic
                }

                disposed = true;
            }
        }

        ~Category()
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
