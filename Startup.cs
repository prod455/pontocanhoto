using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pontocanhoto.Application.Cli;
using Pontocanhoto.EntityFrameworkCore;
using Pontocanhoto.Services;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Extensions.DependencyInjection;

namespace Pontocanhoto
{
    public class Startup
    {
        public void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            string connectionString = configuration.GetConnectionString("PontocanhotoDbConnection") ?? throw new ApplicationException("Missing PontocanhotoDbConnection configuration");

            services.AddDbContext<PontocanhotoDbContext>(options =>
            {
                options.UseSqlServer(connectionString);

                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });

            services.AddSingleton<CommandApp>(provider =>
            {
                ServiceCollection serviceCollection = new ServiceCollection();

                serviceCollection.AddDbContext<PontocanhotoDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);

                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                });

                serviceCollection.AddTransient<RecordService>();
                serviceCollection.AddTransient<PeriodService>();
                serviceCollection.AddTransient<TimesheetService>();

                return new CommandApp(new DependencyInjectionRegistrar(serviceCollection));
            });

            services.AddSingleton<CliApplication>();
        }
    }
}
