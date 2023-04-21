namespace AiTelegramChannel.ServerHost.BackgroundJobs;

public abstract class AbstractBackgroundJob<T> : BackgroundService
{
    protected abstract bool Enabled { get; }

    protected abstract TimeSpan Delay { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!Enabled)
        {
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await RunRecurringJob();

            await Task.Delay(Delay, stoppingToken);
        }
    }

    public abstract Task RunRecurringJob();
}