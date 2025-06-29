using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pontocanhoto.Domain;

namespace Pontocanhoto.EntityFrameworkCore
{
    public class TimesheetModelConf : IEntityTypeConfiguration<TimesheetModel>
    {
        public void Configure(EntityTypeBuilder<TimesheetModel> builder)
        {
            builder.ToTable("Timesheet");

            builder.HasKey(timesheet => timesheet.Id);

            builder.Property(timesheet => timesheet.Id)
                .ValueGeneratedOnAdd();

            builder.Property(timesheet => timesheet.PeriodId);

            builder.Property(timesheet => timesheet.Date)
                .IsRequired();
        }
    }
}
