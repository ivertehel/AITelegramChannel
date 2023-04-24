using AiTelegramChannel.ServerHost.Extensions;
using NLog.Extensions.Logging;
using NLog.Web;

namespace AiTelegramChannel.ServerHost;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.RegisterOptions(builder.Configuration);
        builder.Services.RegisterServices(builder.Configuration);
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddNLog(builder.Configuration);
        });

        builder.Configuration
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        if (builder.Environment.EnvironmentName != null)
        {
            builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
        }

        builder.Host.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Trace);
        }).UseNLog();

        var app = builder.Build();

        app.MapGet("/", () => "The server is working");

        app.Run();
    }
}