using Microsoft.EntityFrameworkCore;
using SampleForElastic.Domain.Models.Outbox;
using SampleForElastic.Domain.Models.UserAggregate;
using SampleForElastic.Infraustructure.Configurations.SampleForElastic.Infrastructure.Configurations;

namespace SampleForElastic.Infraustructure
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }
        public DbSet<User> Users { get; private set; }
        public DbSet<Car> Cars { get; private set; }
        public DbSet<OutboxMessage> OutboxMessages { get; private set; }
    }
}
