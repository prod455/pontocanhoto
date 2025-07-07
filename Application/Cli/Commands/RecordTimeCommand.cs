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
                TimesheetModel? timesheetDb = _timesheetService.GetTimesheetByDate();
                if (timesheetDb != null)
                {
                    _recordService.AddRecord(new RecordModel()
                    {
                        Time = actualDate,
                        TimesheetId = timesheetDb.Id
                    });
                    AnsiConsole.Markup($"[yellow]Timesheet[/]: {timesheetDb.Date:D}\n[yellow]Record[/]: {actualDate:t}");
                }
                else
                {
                    DateTime startDate = new DateTime(actualDate.Year, actualDate.Month, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                    PeriodModel? periodDb = _periodService.GetPeriodByStartEndDate(startDate, endDate);
                    periodDb ??= _periodService.AddPeriod(new PeriodModel
                    {
                        StartDate = startDate,
                        EndDate = endDate
                    });
                    TimesheetModel timesheet = _timesheetService.AddTimesheet(new TimesheetModel()
                    {
                        Date = actualDate,
                        Period = periodDb
                    });
                    _recordService.AddRecord(new RecordModel()
                    {
                        Time = actualDate,
                        TimesheetId = timesheet.Id
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
