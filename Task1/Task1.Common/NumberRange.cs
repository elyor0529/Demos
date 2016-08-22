using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Common
{
    [Serializable]
    public class NumberRange
    { 
        public int Id { get; set; }

        public int Start { get; set; }

        public int End { get; set; } 

        public int Count { get; set; }
          
    }
}
