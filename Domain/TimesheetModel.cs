namespace Pontocanhoto.Domain
{
    public class TimesheetModel
    {
        public int Id { get; set; }
        public int? PeriodId { get; set; }
        public DateTime Date { get; set; }
        public PeriodModel? Period { get; set; }
        public List<RecordModel> Records { get; set; } = new List<RecordModel>();
    }
}
