using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hik.Communication.ScsServices.Service;

namespace Task1.Common
{

    /// <summary>
    /// This interface defines methods of Phone Book Service
    /// that can be called remotely by client applications.
    /// </summary>
    [ScsService(Version = "1.0.0.0")]
    public interface INumberGeneratorService
    {

          int[] GetNumbers(NumberRange range);

    }
}
