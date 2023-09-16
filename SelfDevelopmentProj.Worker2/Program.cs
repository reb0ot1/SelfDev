using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SelfDevelopmentProj.Worker2;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging();
        services.TryAddTransient<InfluxDBService>();
        services.AddHostedService<Worker>();
    })
    .ConfigureLogging(log =>
    {
        log.AddFilter("Microsoft", level => level == LogLevel.Warning);
        log.AddSimpleConsole(c =>
        {
            c.SingleLine = true;
            c.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Enabled;
            c.TimestampFormat = "[HH:mm:ss:ffff] ";
        });
    })
    .Build();

await host.RunAsync();

public class Worker : BackgroundService
{
    private readonly IHost _host;
    private readonly ILogger<Worker> _logger;
    private readonly InfluxDBService _influxDBService;
    private static readonly Random _random = new Random();
    public Worker(IHost host, ILogger<Worker> logger, InfluxDBService infService)
    {
        this._host = host;
        this._logger = logger;
        this._influxDBService = infService;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            this._logger.LogInformation("Start writing");
            try
            {
                this._influxDBService.Write(write =>
                {
                    var point = PointData.Measurement("altitude")
                    .Tag("plane", "test-plane")
                    .Field("value", _random.Next(1000, 5000))
                    .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

                    write.WritePoint(point, "test-bucket", "organization");
                });
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Exception");
            }
            
            this._logger.LogInformation("End writing");
            var ar = Console.ReadLine();
            if (ar == "read")
            {
                try
                {
                    var result = await this._influxDBService.QueryAsync(async query =>
                    {
                        var flux = "from(bucket:\"test-bucket\") |> range(start:0)";
                        var tables = await query.QueryAsync(flux, "organization");
                        //var collection = new List<AltitudeModel>();
                        //foreach (var table in tables)
                        //{
                        //    var records = table.Records;

                        //    collection.AddRange(records.Select(record =>
                        //    {
                        //        var res = new AltitudeModel
                        //        {
                        //            Altitude = int.Parse(record.GetValue().ToString()),
                        //            Time = record.GetTime().ToString()
                        //        };
                        //        return res;
                        //    }));

                        //}

                        return tables.SelectMany(table => table.Records
                                                    .Select(record => new AltitudeModel
                                                    {
                                                        Altitude = int.Parse(record.GetValue().ToString()),
                                                        Time = record.GetTime().ToString()
                                                    }));
                    });

                    if (result.Any())
                    {
                        foreach (var record in result)
                        {
                            Console.WriteLine(record);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Empty collection.");
                    }

                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, "Exception");
                }
                
                
            }

            if (ar == "stop")
            {
                break;
            }
        }

        await this._host.StopAsync();
    }
}