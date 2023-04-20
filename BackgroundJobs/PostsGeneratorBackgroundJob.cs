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

    public PostsGeneratorBackgroundJob(
        IOptions<PostsGeneratorBackgroundJobSettings> jobSettings, 
        IOptions<TelegramSettings> telegramSettings,
        IChatGptClient chatGptClient, 
        ITelegramMessengerClient telegramMessengerClient)
    {
        _jobSettings = jobSettings?.Value ?? throw new ArgumentNullException(nameof(jobSettings));
        _telegramSettings = telegramSettings?.Value ?? throw new AbandonedMutexException(nameof(telegramSettings));
        _chatGptClient = chatGptClient ?? throw new ArgumentNullException(nameof(chatGptClient));
        _telegramMessengerClient = telegramMessengerClient ?? throw new ArgumentNullException(nameof(chatGptClient));
    }

    protected override bool Enabled => _jobSettings.Enabled;

    public override async Task RunRecurringJob()
    {
        var joke = await _chatGptClient.SendMessage(_jobSettings.Message);
        await _telegramMessengerClient.SendMessage(_telegramSettings.ChatId, joke);
    }
}