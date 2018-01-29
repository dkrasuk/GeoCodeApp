using BusinessLayer.Interface;
using CsvHelper;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class CsvService : ICsvService
    {
        public async Task<List<CollateralCsvModel>> ParseCollateralsCsvFile()
        {
            string path = ConfigurationManager.AppSettings["PathCollaterals"];
            Encoding win1251 = Encoding.GetEncoding(1251);

            var collaterals = new List<CollateralCsvModel>();
            try
            {
                using (StreamReader reader = new StreamReader(path, win1251))
                {
                    var csv = new CsvReader(reader);
                    csv.Configuration.Delimiter = ";";
                    csv.Configuration.Encoding = Encoding.UTF8;
                    csv.Configuration.RegisterClassMap(new CollateralMapToCsv());
                    collaterals = csv.GetRecords<CollateralCsvModel>().Where(i => !string.IsNullOrEmpty(i.COLLATERALID)).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return collaterals;
        }
    }
}
