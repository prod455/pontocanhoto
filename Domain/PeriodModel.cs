namespace Pontocanhoto.Domain
{
    public class PeriodModel
    {
        public int Id { get; set; }
        public int TimesheetId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimesheetModel Timesheet { get; set; } = null!;
    }
}
