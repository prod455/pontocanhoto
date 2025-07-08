using Partidoro.Application.Helpers;
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
                DateTime? actualDate = null;
                MethodHelper.Retry(() =>
                {
                    actualDate = _periodService.GetDate();
                });
                if (actualDate == null)
                {
                    throw new ApplicationException("Unable to get system date");
                }

                TimesheetModel? timesheetDb = null;
                MethodHelper.Retry(() =>
                {
                    timesheetDb = _timesheetService.GetTimesheetByDate(actualDate);
                });
                if (timesheetDb != null)
                {
                    MethodHelper.Retry(() => _recordService.AddRecord(new RecordModel()
                    {
                        Time = actualDate.Value,
                        TimesheetId = timesheetDb.Id
                    }));
                    AnsiConsole.Markup($"[yellow]Timesheet[/]: {timesheetDb.Date:D}\n[yellow]Record[/]: {actualDate:t}");
                }
                else
                {
                    DateTime startDate = new DateTime(actualDate.Value.Year, actualDate.Value.Month, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                    PeriodModel? periodDb = null;
                    MethodHelper.Retry(() =>
                    {
                        periodDb = _periodService.GetPeriodByStartEndDate(startDate, endDate);
                    });
                    if (periodDb == null)
                    {
                        periodDb = new PeriodModel
                        {
                            StartDate = startDate,
                            EndDate = endDate
                        };
                        MethodHelper.Retry(() => _periodService.AddPeriod(periodDb));
                    }

                    TimesheetModel timesheet = new TimesheetModel()
                    {
                        Date = actualDate.Value,
                        Period = periodDb
                    };
                    MethodHelper.Retry(() => _timesheetService.AddTimesheet(timesheet));

                    RecordModel record = new RecordModel()
                    {
                        Time = actualDate.Value,
                        TimesheetId = timesheet.Id
                    };
                    MethodHelper.Retry(() => _recordService.AddRecord(record));

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
