using Microsoft.EntityFrameworkCore;
using UserIpService.Core.Entities;

namespace UserIpService.Infrastructure
{
    public class UserIpContext : DbContext
    {
        public DbSet<UserIp> UserIps => Set<UserIp>();

        public UserIpContext(DbContextOptions<UserIpContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserIp>(entity =>
            {
                entity.HasKey(x => new { x.UserId, x.IpText });

                entity.Property(x => x.IpText)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(x => x.FirstSeen).IsRequired();
                entity.Property(x => x.LastSeen).IsRequired();
                entity.Property(x => x.Count).IsRequired();

                entity.HasIndex(x => x.IpText);
            });
        }
    }
}
