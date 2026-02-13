namespace NGC.Logging.DataAccess.Context
{
    using Logging.DataAccess.Models;
    using Microsoft.EntityFrameworkCore;

    internal class LoggingDbContext : DbContext
    {
        public LoggingDbContext(DbContextOptions<LoggingDbContext> options) 
            : base(options)
        {
        }

        // Map only the tables you need
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogLevel> LogLevels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Log entity
            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("Logs"); // Map to existing table in DB
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Module)
                    .HasMaxLength(100)
                    .IsRequired(false);

                entity.Property(e => e.SessionId)
                    .HasDefaultValue(0);

                entity.Property(e => e.Message)
                    .HasMaxLength(1000)
                    .IsRequired();

                entity.Property(e => e.LogLevelId)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .IsRequired();

                // Configure relationship with LogLevel
                entity.HasOne(e => e.LogLevel)
                    .WithMany()
                    .HasForeignKey(e => e.LogLevelId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure LogLevel entity
            modelBuilder.Entity<LogLevel>(entity =>
            {
                entity.ToTable("LogLevels"); // Map to existing table in DB
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Keyword)
                    .HasMaxLength(50)
                    .IsRequired();
            });
        }
    }
}
