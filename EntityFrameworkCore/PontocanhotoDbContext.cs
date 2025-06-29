using Microsoft.EntityFrameworkCore;
using Pontocanhoto.Domain;

namespace Pontocanhoto.EntityFrameworkCore
{
    public class PontocanhotoDbContext : DbContext
    {
        public DbSet<PeriodModel> Periods { get; set; }
        public DbSet<TimesheetModel> Timesheets { get; set; }
        public DbSet<RecordModel> Records { get; set; }

        public PontocanhotoDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PeriodModelConf());
            modelBuilder.ApplyConfiguration(new TimesheetModelConf());
            modelBuilder.ApplyConfiguration(new RecordModelConf());

            base.OnModelCreating(modelBuilder);
        }
    }
}
