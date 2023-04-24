using AiTelegramChannel.ServerHost.Imgur;
using AiTelegramChannel.ServerHost.OpenAi;
using AiTelegramChannel.ServerHost.Options;
using AiTelegramChannel.ServerHost.Telegram;
using Microsoft.Extensions.Options;

namespace AiTelegramChannel.ServerHost.BackgroundJobs;

public class PostsGeneratorBackgroundJob : AbstractBackgroundJob<PostsGeneratorBackgroundJob>
{
    private readonly PostsGeneratorBackgroundJobSettings _jobSettings;
    private readonly ITelegramClient _telegramClient;
    private readonly IChatGptClient _chatGptClient;
    private readonly IUnsplashClient _unsplashClient;

    protected override bool Enabled => _jobSettings.Enabled;
    protected override TimeSpan Delay => TimeSpan.FromMinutes(new Random().Next(_jobSettings.DelayInMinutesFrom, _jobSettings.DelayInMinutesTo));

    public PostsGeneratorBackgroundJob(
        IOptions<PostsGeneratorBackgroundJobSettings> jobSettings,
        IChatGptClient chatGptClient,
        ITelegramClient telegramClient,
        IUnsplashClient unsplashClient,
        ILogger<PostsGeneratorBackgroundJob> logger) : base(logger)
    {
        _jobSettings = jobSettings?.Value ?? throw new ArgumentNullException(nameof(jobSettings));
        _chatGptClient = chatGptClient;
        _telegramClient = telegramClient;
        _unsplashClient = unsplashClient;
    }

    public override async Task RunRecurringJob()
    {
        var chatGptPostResponse = await _chatGptClient.SendMessage(_jobSettings.Message);

        if (_jobSettings.GenerateImages)
        {
            await PostMessageWithImage(chatGptPostResponse);
            return;
        }

        await _telegramClient.PostSimpleMessage(chatGptPostResponse);
    }

    private async Task PostMessageWithImage(string text)
    {
        var chatGptKeywordResponse = await _chatGptClient.SendMessage($"What this text is about in one English word: {text}");

        var keyword = chatGptKeywordResponse.Replace(".", "").Split(" ").First();

        var unsplashResponse = await _unsplashClient.GetRandomImageUrl(keyword);
        if (unsplashResponse.IsFailed)
        {
            await _telegramClient.PostSimpleMessage(text);
            Logger.LogError($"Unsplash returned error {unsplashResponse.Errors.First().Message}");
            return;
        }

        await _telegramClient.PostMessageWithImage(text, new Uri(unsplashResponse.Value));
    }
}