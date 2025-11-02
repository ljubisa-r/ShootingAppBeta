using Microsoft.EntityFrameworkCore;
using GlossaryAPI.Models; 
using GlossaryAPI.DTOs;

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

            modelBuilder.Entity<GlossaryTermDTO>()
                .Property(e => e.status)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .HasConversion<string>();

            modelBuilder.Entity<GlossaryTerm>()
                .HasOne(t => t.Creator)
                .WithMany()
                .HasForeignKey(t => t.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<GlossaryTerm> GlossaryTerms { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!; 
    }
}
