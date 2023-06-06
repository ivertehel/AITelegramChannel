using AiTelegramChannel.ServerHost.Cache;
using AiTelegramChannel.ServerHost.Extensions;
using AiTelegramChannel.ServerHost.Imgur;
using AiTelegramChannel.ServerHost.OpenAi;
using AiTelegramChannel.ServerHost.Options;
using Microsoft.Extensions.Options;
using System.Web;

namespace AiTelegramChannel.ServerHost.BackgroundJobs;

public class PostsGeneratorBackgroundJob : AbstractBackgroundJob<PostsGeneratorBackgroundJob>
{
    private readonly PostsGeneratorBackgroundJobSettings _jobSettings;
    private readonly IChatGptClient _chatGptClient;
    private readonly IUnsplashClient _unsplashClient;
    private readonly InMemoryContext _context;

    protected override bool Enabled => _jobSettings.Enabled;
    protected override TimeSpan Delay => TimeSpan.FromMinutes(_jobSettings.DelayBetweenExecutions);

    public PostsGeneratorBackgroundJob(
        IOptions<PostsGeneratorBackgroundJobSettings> jobSettings,
        IChatGptClient chatGptClient,
        IUnsplashClient unsplashClient,
        InMemoryContext context,
        ILogger<PostsGeneratorBackgroundJob> logger) : base(logger)
    {
        _jobSettings = jobSettings?.Value ?? throw new ArgumentNullException(nameof(jobSettings));
        _chatGptClient = chatGptClient;
        _unsplashClient = unsplashClient;
        _context = context;
    }

    public override async Task RunRecurringJob()
    {
        Logger.TraceEnter();
        var chatGptPostResponse = await _chatGptClient.SendMessage(_jobSettings.Message);

        if (_jobSettings.GenerateImages)
        {
            await CreatePostMessageWithImage(chatGptPostResponse);
            await _context.SaveChangesAsync();
            Logger.TraceExit();
            return;
        }

        _context.Add(new PublicationEntity
        {
            Content = chatGptPostResponse,
            CreatedOn = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        Logger.TraceExit();
    }

    private async Task CreatePostMessageWithImage(string text)
    {
        Logger.TraceEnter(argument: text);

        var chatGptQueryResponse = await _chatGptClient.SendMessage($"What this text is about in two English words: {text}");
        var query = HttpUtility.UrlEncode(chatGptQueryResponse.Replace(".", ""));

        var unsplashResponse = await _unsplashClient.GetImageUrl(query);
        if (unsplashResponse.IsFailed)
        {
            _context.Add(new PublicationEntity
            {
                Content = text,
                CreatedOn = DateTime.UtcNow
            });

            Logger.TraceError($"Unsplash returned error {unsplashResponse.Errors.First().Message}");
            return;
        }

        _context.Add(new PublicationEntity
        {
            Content = text,
            Image = unsplashResponse.Value,
            CreatedOn = DateTime.UtcNow
        });

        Logger.TraceExit();
    }
}