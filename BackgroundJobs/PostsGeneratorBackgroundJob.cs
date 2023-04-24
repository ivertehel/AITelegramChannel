using AiTelegramChannel.ServerHost.Imgur;
using AiTelegramChannel.ServerHost.OpenAi;
using AiTelegramChannel.ServerHost.Options;
using Microsoft.Extensions.Options;
using Telegram.Bots;
using Telegram.Bots.Requests;
using Telegram.Bots.Types;

namespace AiTelegramChannel.ServerHost.BackgroundJobs;

public class PostsGeneratorBackgroundJob : AbstractBackgroundJob<PostsGeneratorBackgroundJob>
{
    private PostsGeneratorBackgroundJobSettings _jobSettings;
    private TelegramSettings _telegramSettings;
    private IBotClient _telegramBotClient;
    private IChatGptClient _chatGptClient;
    private IUnsplashClient _unsplashClient;

    protected override bool Enabled => _jobSettings.Enabled;
    protected override TimeSpan Delay => TimeSpan.FromMinutes(new Random().Next(_jobSettings.DelayInMinutesFrom, _jobSettings.DelayInMinutesTo));

    public PostsGeneratorBackgroundJob(
        IOptions<PostsGeneratorBackgroundJobSettings> jobSettings,
        IOptions<TelegramSettings> telegramSettings,
        IChatGptClient chatGptClient,
        IBotClient botClient,
        IUnsplashClient unsplashClient,
        ILogger<PostsGeneratorBackgroundJob> logger) : base(logger)
    {
        _jobSettings = jobSettings?.Value ?? throw new ArgumentNullException(nameof(jobSettings));
        _telegramSettings = telegramSettings?.Value ?? throw new AbandonedMutexException(nameof(telegramSettings));
        _chatGptClient = chatGptClient;
        _telegramBotClient = botClient;
        _unsplashClient = unsplashClient;
    }

    public override async Task RunRecurringJob()
    {
        var chatGptPostResponse = await _chatGptClient.SendMessage(_jobSettings.Message);
        if (chatGptPostResponse.IsFailed)
        {
            return;
        }

        if (_jobSettings.GenerateImages)
        {
            var chatGptKeywordResponse = await _chatGptClient.SendMessage($"What this text is about in one English word: {chatGptPostResponse.Value}");
            if (chatGptKeywordResponse.IsFailed)
            {
                return;
            }

            var keyword = chatGptKeywordResponse.Value;

            keyword = keyword.Replace(".", "").Split(" ").First();

            var unsplashResponse = await _unsplashClient.GetFirstImageUrl(keyword);
            if (unsplashResponse.IsSuccess)
            {
                var request = new SendMediaGroup(_telegramSettings.ChatId, new List<IGroupableMedia>
                {
                    new PhotoUrl(new Uri(unsplashResponse.Value))
                    {
                        Caption = chatGptPostResponse.Value
                    }
                })
                {
                    DisableNotification = true
                };

                await _telegramBotClient.HandleAsync(request);
                return;
            }
        }

        await _telegramBotClient.HandleAsync(new SendText(_telegramSettings.ChatId, chatGptPostResponse.Value));
    }
}