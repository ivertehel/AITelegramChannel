namespace AiTelegramChannel.ServerHost.BackgroundJobs;

public abstract class AbstractBackgroundJob<T> : BackgroundService
{
    protected abstract bool Enabled { get; }

    public abstract Task RunRecurringJob();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!Enabled)
        {
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await RunRecurringJob();

            await Task.Delay(TimeSpan.FromMinutes(new Random().Next(60, 120)), stoppingToken);
        }
    }
}