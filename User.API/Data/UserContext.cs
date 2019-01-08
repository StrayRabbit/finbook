using Microsoft.EntityFrameworkCore;
using User.API.Models;

namespace User.API.Data
{
    public class UserContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserTag> UserTags { get; set; }
        public DbSet<UserProperty> UserProperties { get; set; }


        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("User").HasKey(e => e.Id);
            });

            modelBuilder.Entity<UserProperty>().Property(u => u.Key).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>().Property(u => u.Value).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>()
                .ToTable("UserProperty")
                .HasKey(u => new { u.Key, u.AppUserId, u.Value });

            modelBuilder.Entity<UserTag>().Property(u => u.Tag).HasMaxLength(100);
            modelBuilder.Entity<UserTag>()
                .ToTable("UserTag")
                .HasKey(u => new { u.UserId, u.Tag });

            modelBuilder.Entity<BPFile>()
                .ToTable("UserBPFile")
                .HasKey(u => new { u.Id });

            base.OnModelCreating(modelBuilder);
        }
    }
}
