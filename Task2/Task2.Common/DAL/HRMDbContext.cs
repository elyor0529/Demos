using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityFramework.DynamicFilters;
using Task2.Common.DAL.EF;
using Task2.Common.DAL.Models;

namespace Task2.Common.DAL
{
    public class HRMDbContext : DbContext
    {
        public HRMDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Developer> Developers { get; set; }

        public DbSet<Salary> Salaries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //mapping 
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Developer>()
                .MapToStoredProcedures(s => s
                    .Update(u => u.HasName("[dbo].[Product_Update_Developer]"))
                    .Delete(d => d.HasName("[dbo].[Product_Delete_Developer]"))
                    .Insert(i => i.HasName("[dbo].[Product_Insert_Developer]")));
              
            //property filter
            modelBuilder.Filter("IsDeleted", (BaseEntity f) => f.IsDeleted, false);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => SaveChanges(), cancellationToken);
        }

        public override int SaveChanges()
        {
            try
            {
                var modifiedEntries = ChangeTracker.Entries()
                    .Where(x => x.Entity is BaseEntity
                                &&
                                (x.State == EntityState.Added || x.State == EntityState.Modified ||
                                 x.State == EntityState.Deleted));

                foreach (var entry in modifiedEntries)
                {
                    var entity = (BaseEntity)entry.Entity;
                    var now = DateTime.Now;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedDate = now;
                    }
                    else
                    {
                        Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedDate = now;
                }

                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}
