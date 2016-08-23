using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task2.Common.Models
{
    public class Salary
    {

        public int Id { get; set; }

        public long? WorkerId { get; set; }
          
        public DateTime? Date { get; set; }

        public decimal? Price { get; set; }
    }
}
