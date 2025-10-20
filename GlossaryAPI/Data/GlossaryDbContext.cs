using Microsoft.EntityFrameworkCore;
using GlossaryAPI.Models;   

namespace GlossaryAPI.Data
{
    public class GlossaryDbContext : DbContext
    {
        public GlossaryDbContext(DbContextOptions<GlossaryDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GlossaryTerm>()
                .Property(e => e.Status)
                .HasConversion<string>();
           
            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .HasConversion<string>();
        }

        public DbSet<GlossaryTerm> GlossaryTerms { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!; 
    }
}
