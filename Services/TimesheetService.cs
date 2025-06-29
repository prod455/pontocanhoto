using Microsoft.EntityFrameworkCore;
using Pontocanhoto.Domain;
using Pontocanhoto.EntityFrameworkCore;

namespace Pontocanhoto.Services
{
    public class TimesheetService
    {
        private readonly PontocanhotoDbContext _pontocanhotoDbContext;

        public TimesheetService(PontocanhotoDbContext pontocanhotoDbContext)
        {
            _pontocanhotoDbContext = pontocanhotoDbContext;
        }

        public TimesheetModel AddTimesheet(TimesheetModel timesheet)
        {
            _pontocanhotoDbContext.Timesheets.Add(timesheet);
            _pontocanhotoDbContext.SaveChanges();
            return timesheet;
        }

        public void UpdateTimesheet(TimesheetModel timesheet)
        {
            _pontocanhotoDbContext.Timesheets.Update(timesheet);
            _pontocanhotoDbContext.SaveChanges();
        }

        public TimesheetModel? GetTimesheetByDate(DateTime? date = null)
        {
            if (date == null)
                date = DateTime.Now;
            return _pontocanhotoDbContext.Timesheets
                .Include(timesheet => timesheet.Period)
                .Include(timesheet => timesheet.Records)
                .Where(timesheet => timesheet.Date.Date == date.Value.Date)
                .ToList()
                .FirstOrDefault();
        }
    }
}
