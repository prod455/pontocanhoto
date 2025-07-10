using Pontocanhoto.Application.Cli.Commands;
using Spectre.Console.Cli;

namespace Pontocanhoto.Application.Cli
{
    public class CliApplication
    {
        private readonly CommandApp _commandApp;

        public CliApplication(CommandApp commandApp)
        {
            _commandApp = commandApp;
            _commandApp.Configure(config =>
            {
                config.AddBranch("period", config =>
                {
                    ListPeriodsCommand(config);
                });
                config.AddBranch("record", config =>
                {
                    RecordTimeCommand(config);
                });
                config.AddBranch("timesheet", config =>
                {
                    ViewTimesheetCommand(config);
                    UpdateTimesheetCommand(config);
                });
            });
        }

        public void Run(IEnumerable<string> args)
        {
            _commandApp.Run(args);
        }

        #region Period
        private void ListPeriodsCommand(IConfigurator<ListPeriodsCommand.Settings> configurator)
        {
            configurator.AddCommand<ListPeriodsCommand>("list");
        }
        #endregion

        #region Record
        private void RecordTimeCommand(IConfigurator<RecordTimeCommand.Settings> configurator)
        {
            configurator.AddCommand<RecordTimeCommand>("time");
        }
        #endregion

        #region Timesheet
        private void ViewTimesheetCommand(IConfigurator<ViewTimesheetCommand.Settings> configurator)
        {
            configurator.AddCommand<ViewTimesheetCommand>("view");
        }

        private void UpdateTimesheetCommand(IConfigurator<UpdateTimesheetCommand.Settings> configurator)
        {
            configurator.AddCommand<UpdateTimesheetCommand>("update");
        }
        #endregion
    }
}
