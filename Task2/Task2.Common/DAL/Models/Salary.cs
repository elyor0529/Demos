using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2.Common.DAL.EF;

namespace Task2.Common.DAL.Models
{
    public class Salary : BaseEntity
    {
        public long? WorkerId { get; set; }

        [ForeignKey("WorkerId")]
        public Developer Worker { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Date { get; set; }

        public decimal? Price { get; set; }
    }
}
