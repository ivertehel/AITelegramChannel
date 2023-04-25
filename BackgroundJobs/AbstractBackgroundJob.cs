using AiTelegramChannel.ServerHost.Extensions;

namespace AiTelegramChannel.ServerHost.BackgroundJobs;

public abstract class AbstractBackgroundJob<T> : BackgroundService
{
    protected abstract bool Enabled { get; }

    protected abstract TimeSpan Delay { get; }

    protected ILogger<T> Logger { get; }

    protected AbstractBackgroundJob(ILogger<T> logger)
    {
        Logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Logger.TraceEnter();

        if (!Enabled)
        {
            Logger.TraceExit();
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunRecurringJob();
            }
            catch(Exception ex)
            {
                Logger.TraceError(ex);
            }

            Logger.LogInformation($"The next execution will be at {DateTime.Now.Add(Delay)}");
            await Task.Delay(Delay, stoppingToken);
        }
    }

    public abstract Task RunRecurringJob();
}