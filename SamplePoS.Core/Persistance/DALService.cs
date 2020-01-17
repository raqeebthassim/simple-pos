using Microsoft.EntityFrameworkCore;
using SamplePoS.Core.Models;

namespace SamplePoS.Core.Persistance
{
    public class DALService
    {
        public static void InitDatabase()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Database.Migrate();
                db.SaveChanges();
            }
        }
    }
}
