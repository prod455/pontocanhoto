namespace Pontocanhoto.Domain
{
    public class RecordModel
    {
        public int Id { get; set; }
        public int TimesheetId { get; set; }
        public DateTime Time { get; set; }
        public TimesheetModel Timesheet { get; set; } = null!;
    }
}
