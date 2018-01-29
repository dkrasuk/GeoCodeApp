using Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BusinessLayer.Interface;
using AlfaBank.Logger;
using log4net;

namespace BusinessLayer.Service
{
    public class GeoCodeService : IGeoCodeService
    {

        private static ILog log = LogManager.GetLogger("LOGGER");
       
        public JObject GetGeoCode(string address, string collateralId, string apiKey)
        {
            try
            {
                WebClient client = new WebClient();
                client.Proxy = new WebProxy("wsproxy.alfa.bank.int", 3128);
                string _url = client.DownloadString(string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}", address, apiKey));
                JObject response = JObject.Parse(_url);
                var location = response["results"][0]["geometry"]["location"] as JObject;
                return location;
            }
            catch (Exception ex)
            {
                Logger.Log.ErrorFormat($"{collateralId.Replace(" ", "")}   -   {address}");
                return null;
            }
            
        }
    }
}
