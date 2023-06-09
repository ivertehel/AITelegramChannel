﻿using AiTelegramChannel.ServerHost.Cache;
using AiTelegramChannel.ServerHost.Extensions;
using AiTelegramChannel.ServerHost.Options;
using AiTelegramChannel.ServerHost.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AiTelegramChannel.ServerHost.BackgroundJobs;

public class PostPublisherBackgroundJob : AbstractBackgroundJob<PostPublisherBackgroundJob>
{
    private readonly PostsPublisherBackgroundJobSettings _jobSettings;
    private readonly ITelegramClient _telegramClient;
    private readonly InMemoryContext _context;

    protected override bool Enabled => _jobSettings.Enabled;

    protected override TimeSpan Delay => TimeSpan.FromSeconds(1);

    public PostPublisherBackgroundJob(
        IOptions<PostsPublisherBackgroundJobSettings> jobSettings,
        ITelegramClient telegramClient,
        InMemoryContext context,
        ILogger<PostPublisherBackgroundJob> logger) : base(logger)
    {
        _jobSettings = jobSettings?.Value ?? throw new ArgumentNullException(nameof(jobSettings));
        _telegramClient = telegramClient;
        _context = context;
    }

    public override async Task RunRecurringJob()
    {
        Logger.TraceEnter();
        var publication = await _context.Publications.OrderBy(p => p.CreatedOn).FirstOrDefaultAsync();

        if (publication == null)
        {
            Logger.TraceExit();
            return;
        }

        if (publication.Image != null)
        {
            await _telegramClient.PostMessageWithImage(publication.Content, new Uri(publication.Image));
        }
        else
        { 
            await _telegramClient.PostSimpleMessage(publication.Content);
        }

        _context.Remove(publication);
        await _context.SaveChangesAsync();

        Logger.TraceExit();
    }
}
