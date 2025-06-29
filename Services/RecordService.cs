using Pontocanhoto.Domain;
using Pontocanhoto.EntityFrameworkCore;

namespace Pontocanhoto.Services
{
    public class RecordService
    {
        private readonly PontocanhotoDbContext _pontocanhotoDbContext;

        public RecordService(PontocanhotoDbContext pontocanhotoDbContext)
        {
            _pontocanhotoDbContext = pontocanhotoDbContext;
        }

        public void AddRecord(RecordModel record)
        {
            _pontocanhotoDbContext.Records.Add(record);
            _pontocanhotoDbContext.SaveChanges();
        }

        public void UpdateRecord(RecordModel record)
        {
            _pontocanhotoDbContext.Records.Update(record);
            _pontocanhotoDbContext.SaveChanges();
        }

        public RecordModel? GetRecord(int id)
        {
            return _pontocanhotoDbContext.Records
                .Where(record => record.Id == id)
                .ToList()
                .FirstOrDefault();
        }
    }
}
