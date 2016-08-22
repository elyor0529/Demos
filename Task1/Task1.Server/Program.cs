using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using Task1.Common;

namespace Task1.Server
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            //Create a Scs Service application that runs on 10048 TCP port.
            var server = ScsServiceBuilder.CreateService(new ScsTcpEndPoint(10085));

            //Add Service to service application
            server.AddService<INumberGeneratorService, NumberGeneratorService>(new NumberGeneratorService());
            server.ClientConnected+=ServerOnClientConnected;
            server.ClientDisconnected+=ServerOnClientDisconnected;

            //Start server
            server.Start();

            //Wait user to stop server by pressing Enter
            Console.WriteLine("Server started successfully. Press enter to stop...");
            Console.ReadLine();

            //Stop server
            server.RemoveService<INumberGeneratorService>();
            server.Stop();

        }

        private static void ServerOnClientDisconnected(object sender, ServiceClientEventArgs serviceClientEventArgs)
        {
            Console.WriteLine("Client#{0} diconnected", serviceClientEventArgs.Client.ClientId);
        }

        private static void ServerOnClientConnected(object sender, ServiceClientEventArgs serviceClientEventArgs)
        {
            Console.WriteLine("Client#{0} connected",serviceClientEventArgs.Client.ClientId);
        }
    }
}
