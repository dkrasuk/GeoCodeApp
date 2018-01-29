using CsvHelper.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class CollateralMapToCsv : CsvClassMap<CollateralCsvModel>
    {
        public CollateralMapToCsv()
        {
            try
            {
                Map(m => m.CREDITAGREEMENTID).Name("CREDITAGREEMENTID");
                Map(m => m.COLLATERALID).Name("COLLATERALID");
                Map(m => m.TYPE).Name("TYPE");
                Map(m => m.SETTLEMENTTYPE).Name("SETTLEMENTTYPE");
                Map(m => m.SETTLEMENT).Name("SETTLEMENT");
                Map(m => m.REGION).Name("REGION");
                Map(m => m.DISTRICT).Name("DISTRICT");
                Map(m => m.STREET).Name("STREET");
                Map(m => m.HOUSE).Name("HOUSE");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
