using Microsoft.EntityFrameworkCore;
using Pontocanhoto.Domain;
using Pontocanhoto.EntityFrameworkCore;

namespace Pontocanhoto.Services
{
    public class PeriodService
    {
        private readonly PontocanhotoDbContext _pontocanhotoDbContext;

        public PeriodService(PontocanhotoDbContext pontocanhotoDbContext)
        {
            _pontocanhotoDbContext = pontocanhotoDbContext;
        }

        public PeriodModel AddPeriod(PeriodModel period)
        {
            _pontocanhotoDbContext.Periods.Add(period);
            _pontocanhotoDbContext.SaveChanges();
            return period;
        }

        public void UpdatePeriod(PeriodModel period)
        {
            _pontocanhotoDbContext.Periods.Update(period);
            _pontocanhotoDbContext.SaveChanges();
        }

        public PeriodModel? GetPeriodByStartEndDate(DateTime? startTime = null, DateTime? endDate = null)
        {
            return _pontocanhotoDbContext.Periods
                .Include(period => period.Timesheet)
                .Where(period => startTime == null || period.StartDate.Date >= startTime.Value.Date)
                .Where(period => endDate == null || period.EndDate.Date <= endDate.Value.Date)
                .ToList()
                .FirstOrDefault();
        }

        public List<PeriodModel> GetPeriodsByStartEndDate(DateTime? startTime = null, DateTime? endDate = null)
        {
            return _pontocanhotoDbContext.Periods
                .Include(period => period.Timesheet)
                .Where(period => startTime == null || period.StartDate.Date >= startTime.Value.Date)
                .Where(period => endDate == null || period.EndDate.Date <= endDate.Value.Date)
                .ToList();
        }
    }
}
