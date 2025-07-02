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
                TimesheetModel? timesheet = _timesheetService.GetTimesheetByDate(settings.Date) ?? throw new ApplicationException("Timesheet not found");

                if (timesheet.Records.Count < 2)
                    throw new ApplicationException("Not enough records to display (2)");

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

                for (int i = 0; i < timesheet.Records.Count; i += 2)
                {
                    grid.AddRow(
                        $"{timesheet.Records[i].Time:t}",
                        $"{(i + 1 < timesheet.Records.Count ? timesheet.Records[i + 1].Time.ToShortTimeString() : "[dim]--:--[/]")}"
                    );
                    totalHours += i + 1 < timesheet.Records.Count ? timesheet.Records[i + 1].Time.TimeOfDay - timesheet.Records[i].Time.TimeOfDay : TimeSpan.Zero;
                }

                panel.Header = new PanelHeader($"[yellow]Timesheet[/]: {timesheet.Date:d}\t[yellow]Elapsed time[/]: {totalHours}");

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
