using Pontocanhoto.Domain;
using Pontocanhoto.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Pontocanhoto.Application.Cli.Commands
{
    public class RecordTimeCommand : Command<RecordTimeCommand.Settings>
    {
        private readonly PeriodService _periodService;
        private readonly RecordService _recordService;
        private readonly TimesheetService _timesheetService;

        public RecordTimeCommand(PeriodService periodService, RecordService recordService, TimesheetService timesheetService)
        {
            _periodService = periodService;
            _recordService = recordService;
            _timesheetService = timesheetService;
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                DateTime actualDate = _periodService.GetDate();
                DateTime startDate = new DateTime(actualDate.Year, actualDate.Month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                PeriodModel? periodDb = _periodService.GetPeriodByStartEndDate(startDate, endDate);
                if (periodDb != null)
                {
                    _recordService.AddRecord(new RecordModel()
                    {
                        Time = actualDate,
                        TimesheetId = periodDb.TimesheetId
                    });
                    AnsiConsole.Markup($"[yellow]Timesheet[/]: {periodDb.Timesheet.Date:D}\n[yellow]Record[/]: {actualDate:t}");
                }
                else
                {
                    TimesheetModel timesheet = _timesheetService.AddTimesheet(new TimesheetModel()
                    {
                        Date = actualDate
                    });
                    PeriodModel period = _periodService.AddPeriod(new PeriodModel()
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        Timesheet = timesheet
                    });
                    timesheet.Period = period;
                    _timesheetService.UpdateTimesheet(timesheet);
                    _recordService.AddRecord(new RecordModel()
                    {
                        Time = actualDate,
                        TimesheetId = period.TimesheetId
                    });
                    AnsiConsole.Markup($"[yellow]Timesheet[/]: {actualDate:D}\n[yellow]Record[/]: {actualDate:t}");
                }
                return 0;
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
            }
        }

        public class Settings : CommandSettings
        {
        }
    }
}
