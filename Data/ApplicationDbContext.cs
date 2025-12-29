using DailyJobTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyJobTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Main table
        public DbSet<DailyJob> DailyJobs { get; set; }

        // Lookup tables
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<EngineerName> EngineerNames { get; set; }
        public DbSet<Category> Categories { get; set; }   // ✅ plural property name
        public DbSet<CaseType> CaseTypes { get; set; }
        public DbSet<CasePriority> CasePriorities { get; set; }
        public DbSet<CompanyName> Companies { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // DailyJobs table mapping
            modelBuilder.Entity<DailyJob>(entity =>
            {
                entity.ToTable("DailyJobs");

                entity.Property(e => e.BreakDuration).HasColumnType("time");

                entity.Property(e => e.TimeTrack)
                      .HasColumnType("time")
                      .HasComputedColumnSql(
                        "DATEADD(SECOND, DATEDIFF(SECOND, [ActivityStartTime], [ActivityEndTime]) - ISNULL(DATEDIFF(SECOND, '00:00:00', [BreakDuration]),0), '00:00:00')",
                        stored: false);

                entity.Property(e => e.Category).HasColumnName("Category");
                entity.Property(e => e.SubCategory).HasColumnName("SubCategory");
                entity.Property(e => e.CaseType).HasColumnName("CaseType");
                entity.Property(e => e.CasePriority).HasColumnName("CasePriority");
                entity.Property(e => e.CompanyName).HasColumnName("CompanyName");
            });

            // Lookup table mappings
            modelBuilder.Entity<Shift>(entity =>
            {
                entity.ToTable("Shift");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.ShiftName).HasColumnName("ShiftName");
            });

            modelBuilder.Entity<EngineerName>(entity =>
            {
                entity.ToTable("EngineerName");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnName("EngineerName");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.CategoryName).HasColumnName("CategoryName");
                entity.Property(c => c.SubCategoryName).HasColumnName("SubCategoryName");
            });

            modelBuilder.Entity<CaseType>(entity =>
            {
                entity.ToTable("CaseType");
                entity.HasKey(ct => ct.Id);
                entity.Property(ct => ct.TypeName).HasColumnName("CaseTypeName");
            });

            modelBuilder.Entity<CasePriority>(entity =>
            {
                entity.ToTable("CasePriority");
                entity.HasKey(cp => cp.Id);
                entity.Property(cp => cp.PriorityName).HasColumnName("PriorityName");
            });

            modelBuilder.Entity<CompanyName>(entity =>
            {
                entity.ToTable("CompanyName");
                entity.HasKey(cn => cn.Id);
                entity.Property(cn => cn.Name).HasColumnName("CompanyName");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.StatusName).HasColumnName("StatusName");
            });
        }
    }
}
