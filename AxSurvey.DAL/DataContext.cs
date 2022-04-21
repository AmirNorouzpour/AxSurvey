using AxSurvey.Common.Utilities;
using AxSurvey.Model.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AxSurvey.DAL
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IEntity).Assembly;
            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);

            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        UserName = "Amir",
                        IsActive = true,
                        Password = "123456789",
                        CreationDate = DateTime.Now,
                        Id = 1
                    }
                );
        }

        public override int SaveChanges()
        {
            var result = 0;
            try
            {
                result = base.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception);
            }

            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var result = base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
