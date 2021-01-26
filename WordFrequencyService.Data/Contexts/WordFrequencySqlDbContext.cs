using Microsoft.EntityFrameworkCore;
using WordFrequencyService.Data.Entities;

namespace WordFrequencyService.Data.Contexts
{
    public class WordFrequencySqlDbContext : DbContext
    {
        public DbSet<WordFrequencyData> WordFrequencies { get; set; }
        public WordFrequencySqlDbContext(DbContextOptions<WordFrequencySqlDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WordFrequencyData>()
                .ToTable("WordFrequency")
                .HasKey(wf => wf.Id);

            modelBuilder.Entity<WordFrequencyData>()
                .HasIndex(wf => wf.Word);

            modelBuilder.Entity<WordFrequencyData>()
                .Property(wf => wf.Word)
                .IsRequired();

            modelBuilder.Entity<WordFrequencyData>()
                .Property(wf => wf.Frequency)
                .IsRequired();

            modelBuilder.Entity<WordFrequencyData>()
                .Property(wf => wf.TotalFrequency)
                .IsRequired();

            modelBuilder.Entity<WordFrequencyData>()
                .Property(wf => wf.Url)
                .IsRequired();

            modelBuilder.Entity<WordFrequencyData>()
                .Property(wf => wf.InsertDate)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
