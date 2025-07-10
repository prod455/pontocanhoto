using Partidoro.Application.Helpers;
using Pontocanhoto.Domain;
using Pontocanhoto.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Pontocanhoto.Application.Cli.Commands
{
    public class UpdateTimesheetCommand : Command<UpdateTimesheetCommand.Settings>
    {
        private readonly TimesheetService _timesheetService;

        public UpdateTimesheetCommand(TimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                DateTime? actualDate = null;
                MethodHelper.Retry(() =>
                {
                    actualDate = _timesheetService.GetDate();
                });
                if (actualDate == null)
                    throw new ApplicationException("Couldn't get actual date");

                TimesheetModel? timesheet = null;
                MethodHelper.Retry(() =>
                {
                    timesheet = _timesheetService.GetTimesheetByDate(settings.TimesheetDate ?? actualDate);
                });
                if (timesheet == null)
                    throw new ApplicationException("Timesheet not found");

                if (settings.Records.Length != timesheet.Records.Count)
                    throw new ApplicationException("Records aren't the same length as saved timesheet records");

                for (int i = 0; i < settings.Records.Length; i++)
                {
                    if (settings.Records[i] > actualDate.Value.TimeOfDay)
                        throw new ApplicationException($"{settings.Records}[i]:hh\\:mm is a future time");

                    if (i > 0)
                    {
                        if (settings.Records[i] < settings.Records[i - 1])
                            throw new ApplicationException($"{settings.Records[i]:hh\\:mm} is a backtrack");
                    }

                    timesheet.Records[i].Time = timesheet.Records[i].Time.Date.Add(settings.Records[i]);
                }

                _timesheetService.UpdateTimesheet(timesheet);

                AnsiConsole.Markup("[yellow]Timesheet updated[/].");

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
            [CommandArgument(0, "[records]")]
            public TimeSpan[] Records { get; set; } = [];

            [CommandOption("-d|--date")]
            public DateTime? TimesheetDate { get; set; }
        }
    }
}
