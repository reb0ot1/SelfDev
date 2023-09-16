// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SelfDevelopmnetProj.ConsoleWithIHost;
using Serilog;
using Serilog.Events;

var host = CreateHostClass.CreateHost();
NumberWorker worker = ActivatorUtilities.CreateInstance<NumberWorker>(host.Services);

worker.PrintNumber();

public static class CreateHostClass
{
    public static IHost CreateHost()
        => Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {
            services.Configure<AppConfiguration>(context.Configuration.GetSection(nameof(AppConfiguration)));
            services.AddSingleton<INumberRepository, NumberRepository>();
            services.AddSingleton<INumberService, NumberService>();
        })
        .UseSerilog((context, services, configuration) => {
            configuration.ReadFrom.Configuration(context.Configuration);
            configuration.ReadFrom.Services(services);
            configuration.Enrich.FromLogContext();
            configuration.WriteTo.Console();
            configuration.WriteTo.File($"report-{DateTimeOffset.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss")}.txt", restrictedToMinimumLevel: LogEventLevel.Warning);
        })
        .Build();
}
