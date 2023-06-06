using Microsoft.EntityFrameworkCore;

namespace AiTelegramChannel.ServerHost.Cache
{
    public class InMemoryContext : DbContext
    {
        public InMemoryContext(DbContextOptions<InMemoryContext> options) : base(options)
        {
        }

        public DbSet<PublicationEntity> Publications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PublicationEntity>(build =>
            {
                build.HasKey(entry => entry.Id);
                build.Property(entry => entry.Id).ValueGeneratedOnAdd();
            });
        }
    }

    public class PublicationEntity
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Image { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}