using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.Common;

namespace Task1.Client.Models
{
    public class GroupModel
    {
        public string Name { get; set; }

        public IEnumerable<NumberRange> Ranges { get; set; }

        public GroupModel()
        {
            Ranges = new NumberRange[] { };
        }
    }
}
