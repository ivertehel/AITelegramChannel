using AiTelegramChannel.ServerHost.OpenAi;
using AiTelegramChannel.ServerHost.Options;
using AiTelegramChannel.ServerHost.Telegram;
using Microsoft.Extensions.Options;

namespace AiTelegramChannel.ServerHost.BackgroundJobs;

public class PostsGeneratorBackgroundJob : AbstractBackgroundJob<PostsGeneratorBackgroundJob>
{
    private PostsGeneratorBackgroundJobSettings _jobSettings;
    private TelegramSettings _telegramSettings;
    private ITelegramMessengerClient _telegramMessengerClient;
    private IChatGptClient _chatGptClient;

    protected override bool Enabled => _jobSettings.Enabled;
    protected override TimeSpan Delay => TimeSpan.FromMinutes(new Random().Next(_jobSettings.DelayInMinutesFrom, _jobSettings.DelayInMinutesTo));

    public PostsGeneratorBackgroundJob(
        IOptions<PostsGeneratorBackgroundJobSettings> jobSettings, 
        IOptions<TelegramSettings> telegramSettings,
        IChatGptClient chatGptClient, 
        ITelegramMessengerClient telegramMessengerClient,
        ILogger<PostsGeneratorBackgroundJob> logger) : base(logger)
    {
        _jobSettings = jobSettings?.Value ?? throw new ArgumentNullException(nameof(jobSettings));
        _telegramSettings = telegramSettings?.Value ?? throw new AbandonedMutexException(nameof(telegramSettings));
        _chatGptClient = chatGptClient ?? throw new ArgumentNullException(nameof(chatGptClient));
        _telegramMessengerClient = telegramMessengerClient ?? throw new ArgumentNullException(nameof(chatGptClient));
        _logger = logger;
    }

    public override async Task RunRecurringJob()
    {
        var message = await _chatGptClient.SendMessage(_jobSettings.Message);
        await _telegramMessengerClient.SendMessage(_telegramSettings.ChatId, message);
    }
}