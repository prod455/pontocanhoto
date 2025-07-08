using Partidoro.Application.Helpers;
using Pontocanhoto.Domain;
using Pontocanhoto.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Pontocanhoto.Application.Cli.Commands
{
    public class ViewTimesheetCommand : Command<ViewTimesheetCommand.Settings>
    {
        private readonly TimesheetService _timesheetService;

        public ViewTimesheetCommand(TimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                TimesheetModel? timesheet = null;
                MethodHelper.Retry(() =>
                {
                    timesheet = _timesheetService.GetTimesheetByDate(settings.Date);
                });
                if (timesheet == null)
                    throw new ApplicationException("Timesheet not found");

                Grid grid = new Grid()
                    .Expand()
                    .AddColumn(new GridColumn().Centered())
                    .AddColumn(new GridColumn().Centered());

                Panel panel = new Panel(grid)
                    .Border(BoxBorder.Rounded)
                    .Expand();

                grid.AddRow("[yellow]In[/]", "[yellow]Out[/]");
                grid.AddEmptyRow();

                TimeSpan totalHours = TimeSpan.Zero;

                DateTime actualTime = _timesheetService.GetDate();

                for (int i = 0; i < timesheet.Records.Count; i += 2)
                {
                    DateTime startTime = timesheet.Records[i].Time;
                    string startTimeString = startTime.ToShortTimeString();

                    if (i + 1 < timesheet.Records.Count)
                    {
                        DateTime endTime = timesheet.Records[i + 1].Time;
                        grid.AddRow(startTimeString, timesheet.Records[i + 1].Time.ToShortTimeString());
                        totalHours += endTime.TimeOfDay - startTime.TimeOfDay;
                    }
                    else
                    {
                        grid.AddRow(startTimeString, "[dim]--:--[/]");

                        if (startTime.Date == actualTime.Date)
                        {
                            totalHours += actualTime.TimeOfDay - startTime.TimeOfDay;
                        }
                    }
                }

                panel.Header = new PanelHeader($"[yellow]Timesheet[/]: {timesheet.Date:d} | [yellow]Elapsed time[/]: {totalHours:hh\\:mm\\:ss}");

                AnsiConsole.Write(panel);

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
            [CommandOption("-d|--date")]
            public DateTime? Date { get; set; }
        }
    }
}
