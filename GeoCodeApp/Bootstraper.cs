using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace GeoCodeApp
{
    public class Bootstraper
    {
        public static void Register(IUnityContainer container)
        {
            BusinessLayer.Bootstraper.Register(container);           
        }
    }
}
