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
        if (!Enabled)
        {
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            Logger.LogInformation($"Running {typeof(T).Name} recurring job");
            await RunRecurringJob();
            Logger.LogInformation($"Recurring job {typeof(T).Name} executed successfully");

            Logger.LogInformation($"The next execution will be at {DateTime.Now.Add(Delay)}");
            await Task.Delay(Delay, stoppingToken);
        }
    }

    public abstract Task RunRecurringJob();
}