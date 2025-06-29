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
                    AddListPeriodsCommand(config);
                });
                config.AddBranch("record", config =>
                {
                    AddRecordTimeCommand(config);
                });
                config.AddBranch("timesheet", config =>
                {
                    ViewTimesheetCommand(config);
                });
            });
        }

        public void Run(IEnumerable<string> args)
        {
            _commandApp.Run(args);
        }

        private void AddListPeriodsCommand(IConfigurator<ListPeriodsCommand.Settings> configurator)
        {
            configurator.AddCommand<ListPeriodsCommand>("list");
        }

        private void AddRecordTimeCommand(IConfigurator<RecordTimeCommand.Settings> configurator)
        {
            configurator.AddCommand<RecordTimeCommand>("time");
        }

        private void ViewTimesheetCommand(IConfigurator<ViewTimesheetCommand.Settings> configurator)
        {
            configurator.AddCommand<ViewTimesheetCommand>("view");
        }
    }
}
