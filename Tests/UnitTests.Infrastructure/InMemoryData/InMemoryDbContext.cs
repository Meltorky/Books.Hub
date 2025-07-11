using Books.Hub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Infrastructure.InMemoryData
{
    public class InMemoryDbContext : AppDbContext
    {
        public InMemoryDbContext() : base(new DbContextOptions<AppDbContext>())
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }
            else 
            { 
                throw new Exception("Configuring Already Exist !!");
            }
        }

        public override void Dispose()
        {
            Database.EnsureDeleted();
            base.Dispose();
        }
    }
}
