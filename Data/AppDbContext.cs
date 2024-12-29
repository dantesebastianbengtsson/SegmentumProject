using Microsoft.EntityFrameworkCore;
using Segmentum.Models;

namespace Segmentum.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Habit> Habits { get; set; }
        public DbSet<Segment> Segments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add configurations here if needed
        }
    }
}
