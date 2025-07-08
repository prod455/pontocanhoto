using Partidoro.Application.Helpers;
using Pontocanhoto.Domain;
using Pontocanhoto.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Pontocanhoto.Application.Cli.Commands
{
    public class ListPeriodsCommand : Command<ListPeriodsCommand.Settings>
    {
        private readonly PeriodService _periodService;

        public ListPeriodsCommand(PeriodService periodService)
        {
            _periodService = periodService;
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


                List<PeriodModel> periods = new List<PeriodModel>();
                MethodHelper.Retry(() =>
                {
                    periods = _periodService.GetPeriodsByStartEndDate(
                        new DateTime(actualDate.Value.Year, 1, 1),
                        actualDate.Value.AddYears(1).AddDays(-1)
                    );
                });

                Grid grid = new Grid()
                    .Expand()
                    .AddColumn(new GridColumn().Centered())
                    .AddColumn(new GridColumn().Centered());

                Panel panel = new Panel(grid)
                    .Header($"[yellow]Period[/]: {actualDate:yyyy}")
                    .Border(BoxBorder.Rounded)
                    .Expand();

                grid.AddRow("[yellow]Start[/]", "[yellow]End[/]");
                grid.AddEmptyRow();

                foreach (PeriodModel period in periods)
                {
                    grid.AddRow(period.StartDate.ToLongDateString(), period.EndDate.ToLongDateString());
                }

                AnsiConsole.Write(panel);

                if (periods.Count == 0)
                {
                    AnsiConsole.Write("[dim]Periods not found.[/]");
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
