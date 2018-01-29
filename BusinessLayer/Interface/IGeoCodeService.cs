using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IGeoCodeService
    {
        JObject GetGeoCode(string address, string collateralId, string apiKey);
    }
}
