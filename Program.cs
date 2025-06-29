using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pontocanhoto.Application.Cli;

namespace Pontocanhoto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Startup startup = new Startup();
            startup.ConfigureServices(builder.Configuration, builder.Services);

            using IHost host = builder.Build();

            host.Services.GetRequiredService<CliApplication>().Run(args);
        }
    }
}
