using AiTelegramChannel.ServerHost.Extensions;
using AiTelegramChannel.ServerHost.Imgur;
using AiTelegramChannel.ServerHost.OpenAi;
using AiTelegramChannel.ServerHost.Options;
using AiTelegramChannel.ServerHost.Telegram;
using Microsoft.Extensions.Options;
using System.Web;

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
        Logger.TraceEnter();
        var chatGptPostResponse = await _chatGptClient.SendMessage(_jobSettings.Message);

        if (_jobSettings.GenerateImages)
        {
            await PostMessageWithImage(chatGptPostResponse);
            Logger.TraceExit();
            return;
        }

        await _telegramClient.PostSimpleMessage(chatGptPostResponse);
        Logger.TraceExit();
    }

    private async Task PostMessageWithImage(string text)
    {
        Logger.TraceEnter(argument: text);

        var chatGptQueryResponse = await _chatGptClient.SendMessage($"What this text is about in two English words: {text}");

        var query = HttpUtility.UrlEncode(chatGptQueryResponse.Replace(".", ""));

        var unsplashResponse = await _unsplashClient.GetImageUrl(query);
        if (unsplashResponse.IsFailed)
        {
            await _telegramClient.PostSimpleMessage(text);
            Logger.TraceError($"Unsplash returned error {unsplashResponse.Errors.First().Message}");
            return;
        }

        await _telegramClient.PostMessageWithImage(text, new Uri(unsplashResponse.Value));
        Logger.TraceExit();
    }
}