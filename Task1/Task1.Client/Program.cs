using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hik.Communication.Scs.Client;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Communication.Messages;
using Hik.Communication.ScsServices.Client;
using log4net.Config;
using Task1.Client.Schedulers;
using Task1.Common;

namespace Task1.Client
{
    internal static class Program
    {
        static Program()
        {
            //log4net
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
        }

        private static void Main()
        { 
            //start scheduler
            JobScheduler.Start(); 
        }
    }
}
