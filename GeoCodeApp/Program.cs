using AlfaBank.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using log4net;
using log4net.Config;
using BusinessLayer;

namespace GeoCodeApp
{
    public class Program
    {

        static void Main(string[] args)
        {
            Logger.InitLogger();
            var container = new UnityContainer();
            Bootstraper.Register(container);
            var program = container.Resolve<WorkflowApp>();

            program.Run();

        }
    }
}
