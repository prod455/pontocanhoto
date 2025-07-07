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
            startTime ??= DateTime.Now;
            endDate ??= DateTime.Now;
            return _pontocanhotoDbContext.Periods
                .Where(period => period.StartDate.Date >= startTime.Value.Date && period.EndDate.Date <= endDate.Value.Date)
                .ToList()
                .FirstOrDefault();
        }

        public List<PeriodModel> GetPeriodsByStartEndDate(DateTime? startTime = null, DateTime? endDate = null)
        {
            startTime ??= DateTime.Now;
            endDate ??= DateTime.Now;
            return _pontocanhotoDbContext.Periods
                .Where(period => period.StartDate.Date >= startTime.Value.Date && period.EndDate.Date <= endDate.Value.Date)
                .ToList();
        }

        public DateTime GetDate()
        {
            return _pontocanhotoDbContext
                .Database
                .SqlQuery<DateTime>($"EXEC GetTimezoneDate")
                .AsEnumerable()
                .FirstOrDefault();
        }
    }
}
