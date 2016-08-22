using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hik.Communication.ScsServices.Service;
using Task1.Common;

namespace Task1.Server
{
    internal class NumberGeneratorService : ScsService, INumberGeneratorService
    {
        public int[] GetNumbers(NumberRange range)
        {
            var counter = 0;
            var random = new Random((int)DateTime.Now.Ticks);
            var task = Task<int[]>.Factory.StartNew(() =>
             {
                 var numbers = new int[range.Count];
                 while (counter < range.Count)
                 {
                     numbers[counter] = random.Next(range.Start, range.End);

                     counter++;
                 }

                 return numbers;
             });

            return Task.WaitAll(new Task[] { task }, 1000) ? task.Result : new int[] { };
        }

    }
}
