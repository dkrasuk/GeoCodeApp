using BusinessLayer.Interface;
using BusinessLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace BusinessLayer
{
    public class Bootstraper
    {
        public static void Register(IUnityContainer container)
        {
            CollateralServiceApiClient.Bootstraper.Register(container);           

            DataAccessLayer.Bootstraper.Register(container);
            container.RegisterType<IAddressServices, AddressServices>();
            container.RegisterType<ICsvService, CsvService>();
            container.RegisterType<ICollateralServices, CollateralServices>();
            container.RegisterType<IGeoCodeService, GeoCodeService>();

            AutomapperConfig.RegisterMappings();
        }
    }
}
