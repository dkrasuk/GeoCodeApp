using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DateAccess
{
    public class CollectSmContext : DbContext
    {
        public CollectSmContext()
            : base("CollectSmContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("COLLECTSM");
        }
    }
}
