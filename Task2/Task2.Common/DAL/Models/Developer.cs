using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Task2.Common.DAL.EF;

namespace Task2.Common.DAL.Models
{
    public class Developer : BaseEntity
    {

        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Salary> Salaries { get; set; }

        public Developer()
        {
            Salaries = new HashSet<Salary>();
        }

    }
}
