using System;
using System.Collections.Generic;
using System.Threading;
using Common.Logging;
using Common.Logging.Simple;
using Quartz;
using Quartz.Impl;
using Task1.Client.Jobs;
using Task1.Client.Utils;
using Task1.Common;

namespace Task1.Client.Schedulers
{
    public static class JobScheduler
    {
        public static void Start()
        {
            try
            {
                //common log
                LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter
                {
                    Level = LogLevel.All
                };

                // construct a scheduler factory
                var factory = new StdSchedulerFactory();

                // get a scheduler
                var scheduler = factory.GetScheduler();
                scheduler.Start();

                // define the job and tie it to our HelloJob class 
                var job = JobBuilder.Create<LoggerJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

                // Trigger the job to run now, and then every 60 seconds
                var trigger = TriggerBuilder.Create()
                  .WithIdentity("trigger1", "group1")
                  .StartNow()
                  .WithSimpleSchedule(x => x
                       .WithIntervalInSeconds(1)
                      .RepeatForever())
                  .Build();

                //TODO:http://www.quartz-scheduler.net/documentation/quartz-2.x/tutorial/crontriggers.html
                scheduler.ScheduleJob(job, trigger);

                while (!scheduler.IsStarted)
                {
                    Console.WriteLine("Waiting for scheduler to start.");
                    Thread.Sleep(1000);
                }

                Console.WriteLine("IsStarted={0}", scheduler.IsStarted);
                Console.WriteLine("InstanceId={0}", scheduler.SchedulerInstanceId);
                Console.WriteLine("SchedulerName={0}", scheduler.SchedulerName);
                Console.WriteLine("The scheduler is running. Press any key to stop");

                Console.ReadKey();
                Console.WriteLine("Shutting down scheduler");

                scheduler.Shutdown(false);
                while (!scheduler.IsShutdown)
                {
                    Console.WriteLine("Waiting for scheduler to shutdown.");
                    Thread.Sleep(1000);
                }
                Console.WriteLine("IsShutdown={0}", scheduler.IsShutdown);
                Console.WriteLine("The scheduler has been shutdown.");

            }
            catch (SchedulerException se)
            {
                LogUtil.Logger.Error("Exception", se);
            }
        }
    }
}
