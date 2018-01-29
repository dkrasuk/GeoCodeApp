using AutoMapper;

using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class AutomapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Location, CollateralService.ApiClient.Client.Models.Presentation.Requests.Collateral.Location>();
                cfg.CreateMap<CollateralService.ApiClient.Client.Models.Presentation.Requests.Collateral.Location, Location>();
            });
        }
    }
}
