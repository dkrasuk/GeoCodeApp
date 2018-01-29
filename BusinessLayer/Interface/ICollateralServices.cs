
using CollateralService.ApiClient.Client.Models.Presentation.Requests.Collateral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICollateralServices
    {
        Task<string> PostLocationAsync(string collateralId, Location location);
    }
}
