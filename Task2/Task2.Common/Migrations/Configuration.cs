using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations;
using Task2.Common.DAL;
using Task2.Common.DAL.Models;

namespace Task2.Common.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<HRMDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(HRMDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            var developers = new[]
            {
                new Developer {Id = 1, Name = "Elyor"},
                new Developer {Id = 2, Name = "Aziz"},
                new Developer {Id = 3, Name = "Khurshid"}
            };
            context.Developers.AddOrUpdate(p => p.Id, developers);

            var k = 0;
            for (var i = 0; i < 10 * 1000; i++)
            {
                var salary = new Salary
                {
                    Id = i + 1
                };

                switch (i % 3)
                {
                    case 0:
                        salary.Date = DateTime.Today.AddYears(-1);
                        salary.WorkerId = 1;
                        salary.Price = 1500;
                        break;
                    case 1:
                        salary.Date = DateTime.Today.AddYears(-2);
                        salary.WorkerId = 2;
                        salary.Price = 1000;
                        break;
                    case 2:
                        salary.Date = DateTime.Today.AddYears(-3);
                        salary.WorkerId = 3;
                        salary.Price = 500;
                        break;
                }

                context.Salaries.Add(salary);

                if (k ==100)
                {
                    context.SaveChanges();
                    k = 0;
                }

                k++;
            }

        }
    }
}