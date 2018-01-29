using DataAccessLayer.DateAccess;
using DataAccessLayer.Interface;
using Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Service
{
    public class AgreementService : IAgreementService
    {
        public async Task<ClientAddresses> GetRegistrationAddresses(int agreementId)
        {
            const string query = "SELECT * FROM TABLE((pcg_alfacollection.getAddressesByAgreementId(:AgreementId)))";
            var parameter = new OracleParameter(":AgreementId", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = agreementId
            };
            using (var contex = new CollectSmContext())
            {
                var answer = contex.Database.SqlQuery<ClientAddresses>(query, parameter).ToList().Where(n => n.Owner == OwnerTypeEnum.Client.ToString() && n.Correct == "100").FirstOrDefault();
                return answer;
            }
        }
    }
}
