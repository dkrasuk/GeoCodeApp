
using BusinessLayer.Interface;
using CollateralService.ApiClient.Client;
using CollateralService.ApiClient.Client.Models.Presentation.Requests.Collateral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class CollateralServices : ICollateralServices
    {
        private readonly ICollateralServiceWebAPI _collateralServiceWebApi;
        public CollateralServices(ICollateralServiceWebAPI collateralServiceWebApi)
        {
            _collateralServiceWebApi = collateralServiceWebApi;
        }

        public async Task<string> PostLocationAsync(string collateralId, Location location)
        {
            var locationDto =  _collateralServiceWebApi.Collateral.PostLocation(collateralId, location);
            return locationDto;
        }


    }
}
