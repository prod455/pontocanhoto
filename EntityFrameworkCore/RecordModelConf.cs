using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pontocanhoto.Domain;

namespace Pontocanhoto.EntityFrameworkCore
{
    public class RecordModelConf : IEntityTypeConfiguration<RecordModel>
    {
        public void Configure(EntityTypeBuilder<RecordModel> builder)
        {
            builder.ToTable("Record");

            builder.HasKey(record => record.Id);

            builder.Property(record => record.Id)
                .ValueGeneratedOnAdd();

            builder.Property(record => record.TimesheetId)
                .IsRequired();

            builder.Property(record => record.Time)
                .IsRequired();

            builder.HasOne(record => record.Timesheet)
                .WithMany(timesheet => timesheet.Records)
                .HasConstraintName("FK_Record_Timesheet_TimesheetId")
                .HasForeignKey(record => record.TimesheetId);
        }
    }
}
