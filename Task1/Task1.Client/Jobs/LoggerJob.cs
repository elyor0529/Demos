using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Client;
using Quartz;
using Task1.Client.Helpers;
using Task1.Client.Utils;
using Task1.Common;

namespace Task1.Client.Jobs
{
    public class LoggerJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            var groups = XMLParser.GetGroups();

            groups.AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithDegreeOfParallelism(Environment.ProcessorCount / 2)
                .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                .ForAll(group =>
              {
                    //Create a client object to connect a server on 127.0.0.1 (local) IP and listens 10085 TCP port
                    var client = ScsServiceClientBuilder.CreateClient<INumberGeneratorService>(new ScsTcpEndPoint("127.0.0.1", 10085));

                    //config
                    client.Timeout = -1;
                  client.ConnectTimeout = 3600;

                    //Register to  event to receive messages from server.
                    client.Connected += ClientOnConnected;
                  client.Disconnected += ClientOnDisconnected;

                    //Wait user to press enter 
                    client.Connect();

                  Console.WriteLine("Group#{0}  numbers logged", group.Name);

                  var sb = new StringBuilder();
                  sb.AppendFormat("\r\n----------{0}--------------\r\n", group.Name);

                  foreach (var job in group.Ranges)
                  {
                      var numbers = client.ServiceProxy.GetNumbers(job);
                      sb.AppendFormat("[{0},{1}]{{{2}}} -> {3}\r\n", job.Start, job.End, job.Count, String.Join(",", numbers));

                      Console.WriteLine("\t Job#{0} for {1} numbers logged", job.Id, numbers.Length);
                  }
                  sb.AppendLine("-------------------------------\r\n");

                  LogUtil.Logger.Info(sb);
                  client.Disconnect();

              });

        }

        private static void ClientOnDisconnected(object sender, EventArgs eventArgs)
        {
            Console.WriteLine("Close connection to server");
        }

        private static void ClientOnConnected(object sender, EventArgs eventArgs)
        {
            Console.WriteLine("Connect to the server");
        }
    }
}
