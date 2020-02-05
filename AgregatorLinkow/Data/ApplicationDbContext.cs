using AgregatorLinkow.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgregatorLinkow.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Link> Links { get; set; }
        public DbSet<Vote> Votes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Links)
                .WithOne(l => l.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(v => v.Votes)
                .WithOne(l => l.User);

            modelBuilder.Entity<Link>()
                .HasMany(l=>l.Votes)
                .WithOne(v=>v.Link)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}