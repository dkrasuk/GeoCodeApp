using DataAccessLayer.Interface;
using DataAccessLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace DataAccessLayer
{
    public class Bootstraper
    {
        public static void Register(IUnityContainer container)
        {
            container.RegisterType<IAgreementService, AgreementService>();
        }

    }
}
