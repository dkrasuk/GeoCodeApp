using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DateAccess
{
    class CollateralContext : DbContext
    {
        public CollateralContext()
           : base("CollateralContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("COLLATERALSERVICE");
        }
    }
}
