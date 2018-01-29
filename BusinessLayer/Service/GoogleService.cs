using BusinessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class GoogleService : IGoogleService
    {
        private readonly IGetGeoCodeService _getGeoCodeService;

        private string googleKey;
        public GoogleService(IGetGeoCodeService getGeoCodeService)
        {
            _getGeoCodeService = getGeoCodeService;
            googleKey = ConfigurationManager.AppSettings["GoogleApiKey"];
        }

        public async Task GetLocation(string address)
        {
            var location = await _getGeoCodeService.GetGeoLocations(address, googleKey);
        }
    }
}
