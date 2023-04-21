using AiTelegramChannel.ServerHost.Extensions;

namespace AiTelegramChannel.ServerHost;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.RegisterOptions(builder.Configuration);
        builder.Services.RegisterServices(builder.Configuration);

        builder.Configuration
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        if (builder.Environment.EnvironmentName != null)
        {
            builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
        }

        var app = builder.Build();

        app.MapGet("/", () => "The server is working");

        app.Run();
    }
}